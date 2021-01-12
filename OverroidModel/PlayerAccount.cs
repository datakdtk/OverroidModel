using System;

namespace OverroidModel
{
    /// <summary>
    /// Class to represent a player.
    /// </summary>
    public class PlayerAccount : IEquatable<PlayerAccount>
    {
        /// <summary>
        /// ID string to identify a player.
        /// </summary>
        public string ID { get; }

        /// <param name="iD">ID string to identify a player.</param>
        public PlayerAccount(string iD)
        {
            ID = iD;
        }

        public bool Equals(PlayerAccount? other)
        {
            return other != null && this.ID == other.ID;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID);
        }
    }
}
