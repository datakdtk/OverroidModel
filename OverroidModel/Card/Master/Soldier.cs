using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 11.
    /// </summary>
    public class Soldier : ICardMaster
    {
        ICardEffect? effect;

        public ushort Value => 11;

        public CardName Name => CardName.Soldier;

        public ICardEffect Effect => effect ??= new Rush();

    }
}
