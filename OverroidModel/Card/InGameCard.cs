using OverroidModel.Card.Effects;
using OverroidModel.Card.Master;

namespace OverroidModel.Card
{
    /// <summary>
    /// Value that determines how cards in player's hand looks from the opponent.
    /// </summary>
    public enum CardVisibility
    {
        /// <summary>
        /// Opponent cannot see at all.
        /// </summary>
        Hidden,
        /// <summary>
        /// Opponent can guess you have the card but cannot see where it is.
        /// </summary>
        Guessed,
        /// <summary>
        /// Revealed by card effect. Opponent can see the card and tell where it is.
        /// </summary>
        Hacked,

        /// <summary>
        /// Revealed in battles.
        /// </summary>
        Opened,
    }

    /// <summary>
    /// Cards used in game. It may have temporal state.
    /// </summary>
    public class InGameCard
    {
        ICardMaster data;
        CardVisibility visibility = CardVisibility.Hidden;
        ushort? overriddenValue;
        ICardEffect? overriddenEffect;

        /// <param name="data">Master data of the card</param>
        internal InGameCard(ICardMaster data)
        {
            this.data = data;
        }

        /// <summary>
        /// Card name that identifies each card in game.
        /// </summary>
        public CardName Name => data.Name;

        /// <summary>
        /// Strength of the card. It may be changed by card effects.
        /// </summary>
        public ushort Value => overriddenValue ?? data.Value;

        /// <summary>
        /// Unique effect of the cards. It may be changed by card effects.
        /// </summary>
        public ICardEffect Effect => overriddenEffect ?? data.Effect;

        /// <summary>
        /// Card Value on initial state that is same as the master data.
        /// </summary>
        public ushort DefaultValue => data.Value;

        /// <summary>
        /// Card Value on initial state that is same as the master data.
        /// </summary>
        public ICardEffect DefaultEffect => data.Effect;

        /// <summary>
        /// How cards in player's hand looks from the opponent.
        /// </summary>
        public CardVisibility Visibility => visibility;

        /// <summary>
        /// Marks the card guessable.
        /// Used when card moves from visible area to invisible area for any player's point of view.
        /// If the card has already been revealed, it is still revealed.
        /// </summary>
        internal void SetGuessed()
        {
            if (Visibility != CardVisibility.Hacked)
            {
                visibility = CardVisibility.Guessed;
            }
        }

        /// <summary>
        /// Reveals the card by card effect and make it visible from all players.
        /// </summary>
        internal void RevealByHack() => visibility = CardVisibility.Hacked;

        /// <summary>
        /// Reveals the card in the battle.
        /// </summary>
        internal void Open() => visibility = CardVisibility.Opened;

        /// <summary>
        /// Change the card's value temporally.
        /// </summary>
        /// <param name="v">New value of the card.</param>
        internal void OverrideValue(ushort v) => overriddenValue = v;

        /// <summary>
        /// Change the card's effect temporally.
        /// </summary>
        /// <param name="v">New effect of the card.</param>
        internal void OverrideEffect(ICardEffect e) => overriddenEffect = e;

        /// <summary>
        /// Cancels temporal state changes.
        /// If cards return to hand, call this method.
        /// </summary>
        internal void ReturnToDefault()
        {
            overriddenValue = null;
            overriddenEffect = null;
        }
    }
}
