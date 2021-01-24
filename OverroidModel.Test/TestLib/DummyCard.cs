using OverroidModel.Card;
using OverroidModel.Card.Effects;
using OverroidModel.Card.Master;

namespace OverroidModel.Test.TestLib
{
    class DummyCard : ICardMaster
    {
        readonly ushort value;
        readonly CardName name;

        public DummyCard(ushort value, CardName name = CardName.Unknown)
        {
            this.value = value;
            this.name = name;
        }

        public ushort Value => value;

        public CardName Name => name;

        public ICardEffect Effect => new NoEffect();
    }
}
