using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 9.
    /// </summary>
    public struct Beast : ICardMaster
    {

        public ushort Value => 9;

        public CardName Name => CardName.Beast;

        public ICardEffect Effect => new Morph();

    }
}
