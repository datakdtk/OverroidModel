using System;

namespace OverroidModel.Exceptions
{
    /// <summary>
    /// Thrown when given player does not participate in the game.
    /// </summary>
    class NonGamePlayerException : Exception
    {
        public NonGamePlayerException(PlayerAccount p) : base(string.Format("{0} is not game player", p.ID))
        {
        }
    }
}
