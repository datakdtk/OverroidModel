using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    public class Overroid : ICardMaster
    {
        public ushort Value => 12;

        public CardName Name => CardName.Overroid;
        public ICardEffect Effect => new Singularity();
    }
}
