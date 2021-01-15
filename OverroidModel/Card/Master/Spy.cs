using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 7.
    /// </summary>
    public class Spy : ICardMaster
    {
        ICardEffect? effect;

        public ushort Value => 7;

        public CardName Name => CardName.Spy;

        public ICardEffect Effect => effect ??= new Espionage();
        
    }
}
