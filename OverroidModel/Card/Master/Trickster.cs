using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    public class Trickster : ICardMaster
    {
        public ushort Value => 6;

        public CardName Name => CardName.Trickster;

        public ICardEffect Effect => new Reversal();
    }
}
