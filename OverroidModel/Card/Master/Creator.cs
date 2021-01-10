using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    public class Creator : ICardMaster
    {
        public ushort Value => 3;

        public CardName Name => CardName.Creator;

        public ICardEffect Effect => new Lifemaker();
    }
}
