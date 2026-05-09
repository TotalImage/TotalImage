using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using TotalImage.Changes;
using TotalImage.Partitions;

namespace TotalImage.Containers
{
    /// <summary>
    /// An abstract representation of container files
    /// </summary>
    public abstract class Container : IDisposable
    {
        private PartitionTable? _partitionTable;

        /// <summary>
        /// The backing file containing the image, opened as a memory-mapped file
        /// </summary>
        protected readonly MemoryMappedFile? backingFile;

        /// <summary>
        /// The underlying stream containing the image
        /// </summary>
        protected Stream containerStream;

        /// <summary>
        /// A stream exposing the content of the container file
        /// </summary>
        public abstract Stream Content { get; }

        /// <summary>
        /// The display name of the container.
        /// </summary>
        public abstract string DisplayName { get; }

        /// <summary>
        /// The length of the container file
        /// </summary>
        public long Length => Content.Length;

        /// <summary>
        /// The set of pending mutations that have been staged but not yet committed to disk.
        /// </summary>
        public ChangeSet PendingChanges { get; } = new ChangeSet();

        /// <summary>
        /// Returns the partition table contained within the image
        /// </summary>
        /// <exception cref="InvalidDataException">Thrown if no partition table could be found within the image</exception>
        public PartitionTable PartitionTable
        {
            get
            {
                _partitionTable ??= LoadPartitionTable();
                if (_partitionTable == null)
                {
                    throw new InvalidDataException();
                }

                return _partitionTable;
            }
        }

        /// <summary>
        /// Create a container file from an existing file
        /// </summary>
        /// <param name="path">The location of the image file</param>
        /// <param name="memoryMapping">Should the file be mapped into memory</param>
        protected Container(string path, bool memoryMapping)
        {
            if (memoryMapping)
            {
                backingFile = MemoryMappedFile.CreateFromFile(path, FileMode.Open);

                /* Using 0 for ViewStream size can cause problems when the view ends up being larger than the file (e.g. with VHDs, as the metadata is in
                 * the footer at the end of the file...), so for now let's just set it to file length instead. */
                FileInfo fileInfo = new FileInfo(path);
                containerStream = backingFile.CreateViewStream(0, fileInfo.Length, MemoryMappedFileAccess.Read);
            }
            else
            {
                containerStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
        }

        /// <summary>
        /// Create a container file from a memory stream
        /// </summary>
        /// <param name="stream">The stream containing the image file</param>
        protected Container(Stream stream)
        {
            containerStream = stream;
            containerStream.Position = 0;
        }

        /// <summary>
        /// Load the partition table from the container image
        /// </summary>
        /// <returns>The partition table found in the image</returns>
        protected virtual PartitionTable LoadPartitionTable()
        {
            return PartitionTable.AttemptDetection(this);
        }

        /// <summary>
        /// Whether this container supports writing (i.e. can have pending changes committed).
        /// All concrete container types that implement <see cref="CloneWriteable"/> return <see langword="true"/>.
        /// </summary>
        public abstract bool SupportsWriting { get; }

        /// <summary>
        /// Creates a new writable instance of this container type backed by the given stream.
        /// Used by <see cref="CommitChanges"/> to open the temporary copy for mutation.
        /// </summary>
        /// <param name="stream">A writable stream containing a verbatim copy of this image.</param>
        protected abstract Container CloneWriteable(Stream stream);

        /// <summary>
        /// Applies all pending changes to a temporary copy of the image, then atomically replaces
        /// the file at <paramref name="path"/> with the updated copy. All open handles to the
        /// container are released before the final rename.
        /// </summary>
        /// <param name="path">The destination path for the committed image.</param>
        /// <param name="progress">
        /// An optional progress reporter. Reports <c>(stage, percent)</c> tuples where percent is 0–100.
        /// </param>
        public async Task CommitChanges(string path, IProgress<(string stage, int percent)>? progress = null)
        {
            string tempPath = Path.ChangeExtension(path, ".tmp");
            try
            {
                await using var tempStream = new FileStream(tempPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);

                // Step 1: Copy source image verbatim into the temp file
                progress?.Report(("Preparing", 0));
                Content.Position = 0;
                await Content.CopyToAsync(tempStream);
                tempStream.Position = 0;

                // Step 2: Open the temp copy as a writable container of the same type
                using var tempContainer = CloneWriteable(tempStream);

                // Step 3: Replay each change in submission order
                int total = PendingChanges.Changes.Count;
                for (int i = 0; i < total; i++)
                {
                    progress?.Report(("Applying changes", total == 0 ? 100 : (int)(100.0 * i / total)));
                    ChangeApplicator.Apply(tempContainer, PendingChanges.Changes[i]);
                }

                tempStream.Flush();
                progress?.Report(("Finalising", 99));
            }
            catch
            {
                try { File.Delete(tempPath); } catch (IOException) { /* best effort */ }
                throw;
            }

            // Step 4: Release all file handles and atomically replace the original
            Dispose();
            File.Move(tempPath, path, overwrite: true);
            progress?.Report(("Done", 100));
        }

        /// <summary>
        /// Save out the container to a file. For images with pending changes use
        /// <see cref="CommitChanges"/> instead.
        /// </summary>
        /// <param name="path">The path to save out the image to</param>
        public void SaveImage(string path)
        {
            string? tempPath = Path.ChangeExtension(path, ".tmp");
            using FileStream outStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None);
            containerStream.Position = 0;
            containerStream.CopyTo(outStream);
            outStream.Flush();
            outStream.Dispose();
            Dispose();
            File.Move(tempPath, path, true);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Method is being called by a Dispose method</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                containerStream.Dispose();
                backingFile?.Dispose();
                PendingChanges.Dispose();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
