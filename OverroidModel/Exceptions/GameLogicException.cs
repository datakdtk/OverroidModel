using System;

namespace OverroidModel.Exceptions
{
    /// <summary>
    /// Error of game logic implementation. Should not be thrown from public method.
    /// </summary>
    public class GameLogicException : Exception
    {
        public GameLogicException(string message) : base(message)
        {
        }
    }
}
