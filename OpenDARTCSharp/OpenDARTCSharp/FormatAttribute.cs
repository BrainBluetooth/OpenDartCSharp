using System;

namespace OpenDARTCSharp
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    internal sealed class FormatAttribute : Attribute
    {
        public readonly string format;

        public FormatAttribute(string format)
        {
            this.format = format;
        }
    }
}