using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 1.
    /// </summary>
    public struct Innocence : ICardMaster
    {

        public ushort Value => 1;

        public CardName Name => CardName.Inocent;

        public ICardEffect Effect => new Miracle();

    }
}
