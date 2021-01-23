using OverroidModel.Card;
using OverroidModel.Card.Effects;
using OverroidModel.Card.Master;

namespace OverroidModel.Test.TestLib
{
    class DummyCard : ICardMaster
    {
        readonly CardName name;
        readonly ushort value;

        public DummyCard(CardName name, ushort value)
        {
            this.name = name;
            this.value = value;
        }

        public ushort Value => value;

        public CardName Name => name;

        public ICardEffect Effect => new NoEffect();
    }
}
