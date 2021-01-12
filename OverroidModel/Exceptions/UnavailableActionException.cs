using System;

namespace OverroidModel.Exceptions
{
    /// <summary>
    /// Thrown when given action cannot be resolved.
    /// </summary>
    public class UnavailableActionException : Exception
    {
        public UnavailableActionException(string message) : base(message)
        {
        }
    }
}
