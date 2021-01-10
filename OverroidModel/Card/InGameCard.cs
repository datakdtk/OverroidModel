using OverroidModel.Card.Effects;
using OverroidModel.Card.Master;

namespace OverroidModel.Card
{
    public enum CardVisibility
    {
        Hidden,
        Guessed,
        Revealed,
    }

    public class InGameCard
    {
        readonly ICardMaster data;
        ushort? overriddenValue;
        ICardEffect? overriddenEffect;

        InGameCard(ICardMaster data)
        {
            this.data = data;
            Visibility = CardVisibility.Hidden;
        }

        public CardName Name => data.Name;
        public ushort Value => overriddenValue ?? data.Value;
        public ICardEffect Effect => overriddenEffect ?? data.Effect;
        public ushort DefaultValue => data.Value;
        public ICardEffect DefaultEffect => data.Effect;
        public CardVisibility Visibility { get; private set; }
        public bool Opened { get; internal set; }

        internal void SetGuessed()
        {
            if (Visibility != CardVisibility.Revealed)
            {
                Visibility = CardVisibility.Guessed;
            }
        }

        internal void Reveal() => Visibility = CardVisibility.Revealed;

        internal void OverrideValue(ushort v) => overriddenValue = v;

        internal void OverrideEffect(ICardEffect e) => overriddenEffect = e;

        internal void ReturnToDefault()
        {
            overriddenValue = null;
            overriddenEffect = null;
        }
    }
}
