using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 4.
    /// </summary>
    public struct Doctor : ICardMaster
    {

        public ushort Value => 4;

        public CardName Name => CardName.Doctor;

        public ICardEffect Effect => new Jamming();

    }
}
