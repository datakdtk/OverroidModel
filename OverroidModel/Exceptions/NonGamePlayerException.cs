using System;

namespace OverroidModel.Exceptions
{
    class NonGamePlayerException : Exception
    {
        public NonGamePlayerException(PlayerAccount p) : base(string.Format("{0} is not game player", p.ID))
        {
        }
    }
}
