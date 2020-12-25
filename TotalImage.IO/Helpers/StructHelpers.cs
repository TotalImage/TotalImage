using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TotalImage.Helpers
{
    public static class StructHelpers
    {
        public unsafe static T ReadStruct<T>(this Stream stream) where T : struct
        {
            var size = Marshal.SizeOf<T>();
            byte[] value = new byte[size];
            stream.Read(value, 0, size);

            fixed (byte* valuePtr = value)
            {
                return Marshal.PtrToStructure<T>((IntPtr)valuePtr);
            }
        }

        public unsafe static void WriteStruct<T>(this Stream stream, T obj) where T : struct
        {
            var size = Marshal.SizeOf<T>();
            byte[] value = new byte[size];

            fixed (byte* valuePtr = value)
            {
                Marshal.StructureToPtr<T>(obj, (IntPtr)valuePtr, false);
            }

            stream.Write(value, 0, size);
        }
    }
}
