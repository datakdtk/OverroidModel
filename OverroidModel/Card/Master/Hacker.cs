using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 2;
    /// </summary>
    public struct Hacker : ICardMaster
    {

        public ushort Value => 2;

        public CardName Name => CardName.Hacker;

        public ICardEffect Effect => new Hack();

    }
}
