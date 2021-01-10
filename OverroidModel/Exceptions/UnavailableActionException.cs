using System;

namespace OverroidModel.Exceptions
{
    public class UnavailableActionException : Exception
    {
        public UnavailableActionException(string message) : base(message)
        {
        }
    }
}
