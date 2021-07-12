using System;
using System.Buffers.Binary;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace TotalImage.FileSystems.HSF
{
    /// <summary>
    /// Represents an ISO 9660 primary volume descriptor
    /// </summary>
    public class HsfPrimaryVolumeDescriptor : HsfVolumeDescriptor
    {
        private static readonly char[] trimCharacters = new char[] { '\0', ' ' };

        /// <summary>
        /// The system that can read the first 16 sectors of the image
        /// </summary>
        public string SystemIdentifier { get; }

        /// <summary>
        /// The identifier for the volume
        /// </summary>
        public string VolumeIdentifier { get; }

        /// <summary>
        /// The total space in logical blocks
        /// </summary>
        public uint VolumeSpace { get; }

        /// <summary>
        /// The number of volumes within the volume set
        /// </summary>
        public ushort VolumeSetSize { get; }

        /// <summary>
        /// The number of the current volume within the volume set
        /// </summary>
        public ushort VolumeSequenceNumber { get; }

        /// <summary>
        /// The size of each logical block in bytes
        /// </summary>
        public ushort LogicalBlockSize { get; }

        /// <summary>
        /// The size of the path table
        /// </summary>
        public uint PathTableSize { get; }

        /// <summary>
        /// Offset of the L Path Table
        /// </summary>
        public uint LPathTableOffset { get; }

        /// <summary>
        /// Offset of an optional 2nd occurrence of the L Path Table
        /// </summary>
        public uint LPathTableOffsetOptional1 { get; }

        /// <summary>
        /// Offset of an optional 3rd occurrence of the L Path Table
        /// </summary>
        public uint LPathTableOffsetOptional2 { get; }

        /// <summary>
        /// Offset of an optional 4th occurrence of the L Path Table
        /// </summary>
        public uint LPathTableOffsetOptional3 { get; }

        /// <summary>
        /// Offset of the M Path Table
        /// </summary>
        public uint MPathTableOffset { get; }

        /// <summary>
        /// Offset of an optional 2nd occurrence of the M Path Table
        /// </summary>
        public uint MPathTableOffsetOptional1 { get; }

        /// <summary>
        /// Offset of an optional 3rd occurrence of the M Path Table
        /// </summary>
        public uint MPathTableOffsetOptional2 { get; }

        /// <summary>
        /// Offset of an optional 4th occurrence of the M Path Table
        /// </summary>
        public uint MPathTableOffsetOptional3 { get; }

        /// <summary>
        /// The root directory record of the file system
        /// </summary>
        public HsfFileSystemObject RootDirectory { get; }

        /// <summary>
        /// The identifier for the volume set
        /// </summary>
        public string VolumeSetIdentifier { get; }

        /// <summary>
        /// An identifier for the publisher
        /// </summary>
        public string PublisherIdentifier { get; }

        /// <summary>
        /// An identifier for the entity responsible for preparing the data recorded in the volume set
        /// </summary>
        public string DataPreparerIdentifier { get; }

        /// <summary>
        /// An identifier for the application used to create the volume set
        /// </summary>
        public string ApplicationIdentifier { get; }

        /// <summary>
        /// A file identifier indicating which file, if any, contains copyright information for the volume set
        /// </summary>
        public string CopyrightFileIdentifier { get; }

        /// <summary>
        /// A file identifier indicating which file, if any, contains an abstract for the volume set
        /// </summary>
        public string AbstractFileIdentifier { get; }

        /// <summary>
        /// The time the volume was created
        /// </summary>
        public DateTimeOffset? VolumeCreationTime { get; }

        /// <summary>
        /// The time the volume was last modified
        /// </summary>
        public DateTimeOffset? VolumeModificationTime { get; }

        /// <summary>
        /// The time that data on the volume may be regarded as obsolete
        /// </summary>
        public DateTimeOffset? VolumeExpirationTime { get; }

        /// <summary>
        /// The time that data on the volume may be regarded ready to use
        /// </summary>
        public DateTimeOffset? VolumeEffectiveTime { get; }

        /// <summary>
        /// The verison of the specification of directory records and the path table
        /// </summary>
        public byte FileStructureVersion { get; }

        /// <summary>
        /// Content reserved for application use
        /// </summary>
        public ImmutableArray<byte> ApplicationContent { get; }

        /// <summary>
        /// Create an ISO 9660 primary volume descriptor
        /// </summary>
        /// <param name="record">A span containing the volume descriptor record</param>
        /// <param name="type">The type of the volume descriptor</param>
        /// <param name="identifier">The volume descriptor identifier</param>
        /// <param name="version">The version of the volume descriptor</param>
        public HsfPrimaryVolumeDescriptor(in ReadOnlySpan<byte> record, in HsfVolumeDescriptorType type, in ImmutableArray<byte> identifier, in byte version)
            : base(type, identifier, version)
        {
            Encoding encoding = Encoding.ASCII;

            char[] textBuffer = new char[32];

            encoding.GetChars(record[16..48], textBuffer);
            SystemIdentifier = textBuffer.AsSpan().TrimEnd(trimCharacters).ToString();

            encoding.GetChars(record[48..80], textBuffer);
            VolumeIdentifier = textBuffer.AsSpan().TrimEnd(trimCharacters).ToString();

            VolumeSpace = HsfUtilities.ReadUInt32MultiEndian(record[88..96]);

            VolumeSetSize = HsfUtilities.ReadUInt16MultiEndian(record[128..132]);
            VolumeSequenceNumber = HsfUtilities.ReadUInt16MultiEndian(record[132..136]);

            LogicalBlockSize = HsfUtilities.ReadUInt16MultiEndian(record[136..140]);
            PathTableSize = HsfUtilities.ReadUInt32MultiEndian(record[140..148]);

            LPathTableOffset = BinaryPrimitives.ReadUInt32LittleEndian(record[148..152]);
            LPathTableOffsetOptional1 = BinaryPrimitives.ReadUInt32LittleEndian(record[152..156]);
            LPathTableOffsetOptional2 = BinaryPrimitives.ReadUInt32LittleEndian(record[156..160]);
            LPathTableOffsetOptional3 = BinaryPrimitives.ReadUInt32LittleEndian(record[160..164]);

            MPathTableOffset = BinaryPrimitives.ReadUInt32BigEndian(record[164..168]);
            MPathTableOffsetOptional1 = BinaryPrimitives.ReadUInt32BigEndian(record[168..172]);
            MPathTableOffsetOptional2 = BinaryPrimitives.ReadUInt32BigEndian(record[172..180]);
            MPathTableOffsetOptional3 = BinaryPrimitives.ReadUInt32BigEndian(record[180..188]);

            RootDirectory = new HsfFileSystemObject(record[180..214], true);

            textBuffer = new char[128];

            encoding.GetChars(record[214..342], textBuffer);
            VolumeSetIdentifier = textBuffer.AsSpan().TrimEnd(trimCharacters).ToString();

            encoding.GetChars(record[342..470], textBuffer);
            PublisherIdentifier = textBuffer.AsSpan().TrimEnd(trimCharacters).ToString();

            encoding.GetChars(record[470..598], textBuffer);
            DataPreparerIdentifier = textBuffer.AsSpan().TrimEnd(trimCharacters).ToString();

            encoding.GetChars(record[598..726], textBuffer);
            ApplicationIdentifier = textBuffer.AsSpan().TrimEnd(trimCharacters).ToString();

            textBuffer = new char[37];

            encoding.GetChars(record[726..760], textBuffer);
            CopyrightFileIdentifier = textBuffer.AsSpan().TrimEnd(trimCharacters).ToString();

            encoding.GetChars(record[760..790], textBuffer);
            AbstractFileIdentifier = textBuffer.AsSpan().TrimEnd(trimCharacters).ToString();

            textBuffer = new char[16];

            Encoding.ASCII.GetChars(record[790..806], textBuffer);
            VolumeCreationTime = HsfUtilities.FromHsfDateTime(textBuffer);

            Encoding.ASCII.GetChars(record[806..822], textBuffer);
            VolumeModificationTime = HsfUtilities.FromHsfDateTime(textBuffer);

            Encoding.ASCII.GetChars(record[822..838], textBuffer);
            VolumeExpirationTime = HsfUtilities.FromHsfDateTime(textBuffer);

            Encoding.ASCII.GetChars(record[838..854], textBuffer);
            VolumeEffectiveTime = HsfUtilities.FromHsfDateTime(textBuffer);

            FileStructureVersion = record[854];

            ApplicationContent = record[856..1368].ToArray().ToImmutableArray();
        }
    }
}
