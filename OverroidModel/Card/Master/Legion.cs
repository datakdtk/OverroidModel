using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 10.
    /// </summary>
    public struct Legion : ICardMaster
    {

        public ushort Value => 10;

        public CardName Name => CardName.Legion;

        public ICardEffect Effect => new Trample();

    }
}
