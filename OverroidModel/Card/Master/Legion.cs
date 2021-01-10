using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    public class Legion : ICardMaster
    {
        public ushort Value => 10;

        public CardName Name => CardName.Legion;

        public ICardEffect Effect => new Trample();
    }
}
