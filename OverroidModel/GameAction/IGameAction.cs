using OverroidModel.Card;

namespace OverroidModel.GameAction
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
        public CardName? TargetCardName { get; }

        /// <summary>
        /// Name of the card that is secondary affected by action if exists.
        /// </summary>
        public CardName? SecondTargetCardName => null;

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
