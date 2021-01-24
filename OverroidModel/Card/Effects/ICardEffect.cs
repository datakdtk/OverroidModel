using System;
using OverroidModel.GameAction;

namespace OverroidModel.Card.Effects
{
    /// <summary>
    /// Value that determines when card abilities will be triggered. 
    /// </summary>
    internal enum EffectTiming
    {
        /// <summary>
        /// Just after battle cards are opened, mainly for disabling other card effects.
        /// /// </summary>
        FIRST = 0,
        /// <summary>
        /// Next of FIRST timing, mainly for copying another card effect.
        /// </summary>
        SECOND = 1,
        /// <summary>
        /// For most card effects that will triggers before comparing card values.
        /// </summary>
        PRE_BATTLE = 2,
        /// <summary>
        /// For most card effects that will triggers after comparing card values.
        /// </summary>
        POST_BATTLE = 3,
    }

    /// <summary>
    /// Common interface for card effect classes.
    /// </summary>
    public interface ICardEffect : IEquatable<ICardEffect>
    {

        /// <summary>
        /// Value that determines when card abilities will be triggered. 
        /// </summary>
        internal EffectTiming Timing { get; }

        /// <summary>
        /// Gets action object that changes game state.
        /// </summary>
        /// <param name="sourceCardName">Name of the card from which this effect was triggered.</param>
        /// <param name="g">Current game object</param>
        /// <returns></returns>
        internal IGameAction GetAction(CardName sourceCardName, IGameInformation g);

        /// <summary>
        /// Checks if this effect triggers on current game state. But does not consider effect timing.
        /// </summary>
        /// <param name="sourceCardName">Name of the card from which this effect will be triggered.</param>
        /// <param name="g">Current game object</param>
        /// <returns>Returns true if the trigger condition is satisfied.</returns>
        internal bool ConditionIsSatisfied(CardName sourceCardName, IGameInformation g);

        /// <summary>
        /// Just if they are same class, they are equal. Overriding mainly for test assertion.
        /// </summary>
        bool IEquatable<ICardEffect>.Equals(ICardEffect? other) => other != null && this.GetType() == other.GetType();
    }
}
