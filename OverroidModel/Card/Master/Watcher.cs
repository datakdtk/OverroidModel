using OverroidModel.Card.Effects;

namespace OverroidModel.Card.Master
{
    public class Watcher : ICardMaster
    {
        ICardEffect? effect;

        public ushort Value => 0;

        public CardName Name => CardName.Watcher;

        public ICardEffect Effect => effect ??= new NoEffect(); // Temporal implement
    }
}
