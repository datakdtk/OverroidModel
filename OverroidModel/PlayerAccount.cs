using System;

namespace OverroidModel
{
    /// <summary>
    /// Class to represent a player.
    /// </summary>
    public class PlayerAccount : IEquatable<PlayerAccount>
    {
        readonly string id;
        readonly string displayName;

        /// <param name="id">ID string to identify a player.</param>
        /// <param name="displayName">Player name to show in the game screen.</param>
        public PlayerAccount(string id, string displayName = "unknown")
        {
            this.id = id;
            this.displayName = displayName;
        }

        /// <summary>
        /// ID string to identify a player.
        /// </summary>
        public string ID => id;

        public string DisplayName => displayName;

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
            return ID.GetHashCode();
        }

        public static bool operator ==(PlayerAccount? self, PlayerAccount? other)
        {
            var otherIsNull = (object?)other == null;
            return (object?)self == null ?  otherIsNull : !otherIsNull && self.Equals(other);
        }

        public static bool operator !=(PlayerAccount? self, PlayerAccount? other)
        {
            return !(self == other);
        }
    }
}
