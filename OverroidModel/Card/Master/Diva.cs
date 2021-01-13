using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 8.
    /// </summary>
    public struct Diva : ICardMaster
    {

        public ushort Value => 18;

        public CardName Name => CardName.Diva;

        public ICardEffect Effect => new Inspiration();

    }
}
