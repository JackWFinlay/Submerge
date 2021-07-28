using System;

namespace Submerge.Abstractions.Exceptions
{
    public class SubmergeParseException : Exception
    {
        public SubmergeParseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}