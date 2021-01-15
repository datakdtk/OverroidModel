using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 4.
    /// </summary>
    public class Doctor : ICardMaster
    {
        ICardEffect? effect;

        public ushort Value => 4;

        public CardName Name => CardName.Doctor;

        public ICardEffect Effect => effect ??= new Jamming();

    }
}
