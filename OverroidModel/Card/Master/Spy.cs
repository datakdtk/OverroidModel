using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 7.
    /// </summary>
    public struct Spy : ICardMaster
    {

        public ushort Value => 7;

        public CardName Name => CardName.Spy;

        public ICardEffect Effect => new Espionage();
        
    }
}
