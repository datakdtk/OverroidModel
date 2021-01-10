using System.Collections.Generic;
using OverroidModel.Card;

namespace OverroidModel.Game.Actions
{
    public interface IGameAction
    {
        public PlayerAccount? Controller { get; }

        public CardName? TargetCardName => null;

        public Dictionary<string, string> VisualEffectParameter => new Dictionary<string, string>();

        public bool IsCardEffect();

        public bool HasVisualEffect();

        internal void Resolve(in IGame g);
    }
}
