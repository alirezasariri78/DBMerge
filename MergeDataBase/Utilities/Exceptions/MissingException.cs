using System;

namespace DBDiff.Utilities.Exceptions
{
    public class MissingException : Exception
    {
        public MissingException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
