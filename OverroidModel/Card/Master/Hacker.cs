using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    public class Hacker : ICardMaster
    {
        public ushort Value => 2;

        public CardName Name => CardName.Hacker;

        public ICardEffect Effect => new Hack();
    }
}
