using System;

namespace OverroidModel.Exceptions
{
    class CardNotFoundException : Exception
    {
        public CardNotFoundException(string message) : base(message)
        {
        }
    }
}
