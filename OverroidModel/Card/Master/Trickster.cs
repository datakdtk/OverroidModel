using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 6.
    /// </summary>
    public struct Trickster : ICardMaster
    {

        public ushort Value => 6;

        public CardName Name => CardName.Trickster;

        public ICardEffect Effect => new Reversal();

    }
}
