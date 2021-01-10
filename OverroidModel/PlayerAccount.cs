using System;

namespace OverroidModel
{
    public class PlayerAccount : IEquatable<PlayerAccount>
    {
        public string ID { get; }

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
