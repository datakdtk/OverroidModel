using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 10.
    /// </summary>
    public class Legion : ICardMaster
    {
        ICardEffect? effect;

        public ushort Value => 10;

        public CardName Name => CardName.Legion;

        public ICardEffect Effect => effect ??= new Trample();

    }
}
