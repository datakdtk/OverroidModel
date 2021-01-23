using OverroidModel.Card;
using OverroidModel.Card.Effects;
using OverroidModel.Card.Master;

namespace OverroidModel.Test.TestLib
{
    class DummyCard : ICardMaster
    {
        readonly ushort value;

        public DummyCard(ushort value)
        {
            this.value = value;
        }

        public ushort Value => value;

        public CardName Name => CardName.Unknown;

        public ICardEffect Effect => new NoEffect();
    }
}
