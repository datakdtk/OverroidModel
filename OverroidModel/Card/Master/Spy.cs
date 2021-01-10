using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    public class Spy : ICardMaster
    {
        public ushort Value => 7;

        public CardName Name => CardName.Spy;
        public ICardEffect Effect => new Espionage();
    }
}
