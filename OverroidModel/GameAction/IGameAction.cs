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
        public CardName? SecondTargetCardName { get; }

        /// <summary>
        /// Changes the state of Current game.
        /// </summary>
        /// <param name="g">Game that will be changed by the action.</param>
        internal void Resolve(IMutableGame g);

    }
}
