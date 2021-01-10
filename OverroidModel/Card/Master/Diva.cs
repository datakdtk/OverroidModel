using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    public class Diva : ICardMaster
    {
        public ushort Value => 18;

        public CardName Name => CardName.Diva;

        public ICardEffect Effect => new Inspiration();
    }
}
