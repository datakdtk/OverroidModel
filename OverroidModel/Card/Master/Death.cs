using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    public class Death : ICardMaster
    {
        public ushort Value => 13;

        public CardName Name => CardName.Death;

        public ICardEffect Effect => new Snipe();
    }
}
