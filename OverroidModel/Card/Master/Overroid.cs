using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    /// <summary>
    /// Card with value 12.
    /// </summary>
    public class Overroid : ICardMaster
    {

        public ushort Value => 12;

        public CardName Name => CardName.Overroid;

        public ICardEffect Effect => new Singularity();

    }
}
