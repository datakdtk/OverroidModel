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
            return (object?)other != null && this.ID == other.ID; 
        }

        public override bool Equals(object? obj)
        {
            return this.Equals(obj as PlayerAccount);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID);
        }

        public static bool operator ==(PlayerAccount self, PlayerAccount other)
        {
            return self.Equals(other);
        }

        public static bool operator !=(PlayerAccount self, PlayerAccount other)
        {
            return !(self == other);
        }
    }
}
