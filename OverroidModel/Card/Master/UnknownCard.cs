using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    class UnknownCard : ICardMaster
    {
        ICardEffect? effect;

        public ushort Value => 0;

        public CardName Name => CardName.Unknown;

        public ICardEffect Effect => effect ??= new NoEffect();
    }
}
