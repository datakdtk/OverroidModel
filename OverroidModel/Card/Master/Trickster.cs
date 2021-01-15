using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 6.
    /// </summary>
    public class Trickster : ICardMaster
    {
        ICardEffect? effect;

        public ushort Value => 6;

        public CardName Name => CardName.Trickster;

        public ICardEffect Effect => effect ??= new Reversal();

    }
}
