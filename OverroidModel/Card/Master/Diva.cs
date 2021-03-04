using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 8.
    /// </summary>
    public class Diva : ICardMaster
    {
        ICardEffect? effect;

        public ushort Value => 8;

        public CardName Name => CardName.Diva;

        public ICardEffect Effect => effect ??= new Inspiration();

    }
}
