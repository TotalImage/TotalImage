namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// A simple FAT helper class.
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// Converts the given string to a FAT-style volume label.
        /// </summary>
        /// <param name="label">The string to use as the volume label</param>
        /// <param name="length">Maximum length of the result volume label</param>
        /// <returns></returns>
        public static string UseAsLabel(string label, int length = 11)
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
