using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// interface that provides what is written on cards.
    /// </summary>
    public interface ICardMaster
    {

        /// <summary>
        /// Strength of the card.
        /// </summary>
        public ushort Value { get; }

        /// <summary>
        /// Card name that identifies each card in game.
        /// </summary>
        public CardName Name { get; }
        
        /// <summary>
        /// Unique effect of the card.
        /// </summary>
        public ICardEffect Effect { get; }

    }
}
