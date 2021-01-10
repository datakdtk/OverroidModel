using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    public class Beast : ICardMaster
    {
        public ushort Value => 9;

        public CardName Name => CardName.Beast;

        public ICardEffect Effect => new Morph();
    }
}
