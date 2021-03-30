using System;

namespace TotalImage.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property)]
    public class LengthAttribute : Attribute
    {
        private int MaximumLength { get; }

        public LengthAttribute(int maximumLength)
        {
            MaximumLength = maximumLength;
        }
    }
}