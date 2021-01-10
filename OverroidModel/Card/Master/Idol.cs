using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    public class Idol : ICardMaster
    {
        public ushort Value => 5;

        public CardName Name => CardName.Idol;

        public ICardEffect Effect => new Charm();
    }
}
