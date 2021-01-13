using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 13.
    /// </summary>
    public struct Death : ICardMaster
    {

        public ushort Value => 13;

        public CardName Name => CardName.Death;

        public ICardEffect Effect => new Snipe();

    }
}
