using OverroidModel.Card;

namespace OverroidModel.GameAction.Effects
{
    interface ICardEffectAction : IGameAction
    {
        /// <summary>
        /// Name of the card that this effect is triggered from
        /// </summary>
        public CardName SourceCardName { get;  }
    }
}
