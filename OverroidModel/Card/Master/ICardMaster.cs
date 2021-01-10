using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    public interface ICardMaster
    {
        /// <summary>
        /// Strength of the card
        /// </summary>
        public ushort Value { get; }

        public CardName Name { get; }
        
        public ICardEffect Effect { get; }
        
    }
}
