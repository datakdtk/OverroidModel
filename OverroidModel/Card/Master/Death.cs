using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 13.
    /// </summary>
    public class Death : ICardMaster
    {
        ICardEffect? effect;

        public ushort Value => 13;

        public CardName Name => CardName.Death;

        public ICardEffect Effect => effect ??= new Snipe();

    }
}
