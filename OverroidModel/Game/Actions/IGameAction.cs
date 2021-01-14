using System.Collections.Generic;
using OverroidModel.Card;

namespace OverroidModel.Game.Actions
{
    /// <summary>
    /// interfaces for classes that represent game transition.
    /// </summary>
    public interface IGameAction
    {

        /// <summary>
        /// Player who is responsible for the action if exists. 
        /// </summary>
        public PlayerAccount? Controller { get; }

        /// <summary>
        /// Name of the card that is mainly affected by action if exists.
        /// </summary>
        public CardName? TargetCardName => null;

        /// <summary>
        /// Parameters that are required for render animation.
        /// </summary>
        public Dictionary<string, string> VisualEffectParameter => new Dictionary<string, string>();

        /// <summary>
        /// Whether it is card effect.
        /// </summary>
        public bool IsCardEffect();

        /// <summary>
        /// Whether the result of the action has visual change.
        /// </summary>
        public bool HasVisualEffect();

        /// <summary>
        /// Changes the state of Current game.
        /// </summary>
        /// <param name="g">Game that will be changed by the action.</param>
        internal void Resolve(IMutableGame g);

    }
}
