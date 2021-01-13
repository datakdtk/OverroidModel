using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 5.
    /// </summary>
    public struct Idol : ICardMaster
    {

        public ushort Value => 5;

        public CardName Name => CardName.Idol;

        public ICardEffect Effect => new Charm();
        
    }
}
