using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    class Innocence : ICardMaster
    {
        public ushort Value => 1;

        public CardName Name => CardName.Inocent;

        public ICardEffect Effect => new Miracle();
    }
}
