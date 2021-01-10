using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    public class Soldier : ICardMaster
    {
        public ushort Value => 11;

        public CardName Name => CardName.Soldier;

        public ICardEffect Effect => new Rush();
    }
}
