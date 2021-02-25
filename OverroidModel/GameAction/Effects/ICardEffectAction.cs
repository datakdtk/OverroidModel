using OverroidModel.Card;

namespace OverroidModel.GameAction.Effects
{
    public interface ICardEffectAction : IGameAction
    {
        /// <summary>
        /// Name of the card that this effect is triggered from
        /// </summary>
        public CardName SourceCardName { get;  }
    }
}
