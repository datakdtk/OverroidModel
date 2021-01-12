using System;

namespace OverroidModel.Exceptions
{
    /// <summary>
    /// Thrown when a card is not found in expected zone.
    /// </summary>
    class CardNotFoundException : Exception
    {
        public CardNotFoundException(string message) : base(message)
        {
        }
    }
}
