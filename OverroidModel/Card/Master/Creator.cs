using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 3.
    /// </summary>
    public class Creator : ICardMaster
    {

        public ushort Value => 3;

        public CardName Name => CardName.Creator;

        public ICardEffect Effect => new Lifemaker();

    }
}
