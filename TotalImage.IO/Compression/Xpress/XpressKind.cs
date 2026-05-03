namespace TotalImage.Compression.Xpress
{
    /// <summary>
    /// Supported variants of the Microsoft Xpress compression family.
    /// </summary>
    public enum XpressKind
    {
        /// <summary>
        /// The plain LZ77 Xpress variant documented in MS-XCA section 3.1.
        /// This is the variant used by Windows CE IMGFS <c>XPR</c> chunks.
        /// </summary>
        PlainLz77,
    }
}
