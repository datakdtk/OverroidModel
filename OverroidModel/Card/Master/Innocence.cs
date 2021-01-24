using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 1.
    /// </summary>
    public class Innocence : ICardMaster
    {
        ICardEffect? effect;

        public ushort Value => 1;

        public CardName Name => CardName.Innocence;

        public ICardEffect Effect => effect ??= new Miracle();

    }
}
