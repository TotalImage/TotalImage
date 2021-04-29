using System;
using System.Linq;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    internal static class Helper
    {
        public static string UseAsLabel(string label, int length = 8)
        {
            if (label.Length > length)
            {
                label = label.Substring(0, length);
            }
            else if (label.Length < length)
            {
                label = label.PadRight(length);
            }

            var chars = label.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] >= 128)
                {
                    chars[i] = '_';
                }
                else if (char.IsLower(chars[i]))
                {
                    chars[i] = char.ToUpper(chars[i]);
                }
            }

            return new string(chars);
        }
    }
}