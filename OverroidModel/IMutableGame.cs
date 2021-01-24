using OverroidModel.GameAction;
using OverroidModel.GameAction.Commands;

namespace OverroidModel
{
    /// <summary>
    /// Interface to modify all kind of games.
    /// </summary>
    public interface IMutableGame : IGameInformation
    {

        /// <summary>
        /// Receive and resolve a game command by a player in game.
        /// </summary>
        /// <exception cref="UnavailableActionException">Thrown when the command is not applicable in the current game state.</exception>
        public void ReceiveCommand(IGameCommand command);

        /// <summary>
        /// Create new battle and add it to the game.
        /// </summary>
        internal void AddNewRound();

        /// <summary>
        /// Be ready to accept game command.
        /// </summary>
        /// <param name="a">Object to check commands is applicable.</param>
        internal void AddCommandAuthorizer(ICommandAuthorizer a);

        /// <summary>
        /// Disable card effects of given player and given battle round.
        /// </summary>
        /// <param name="round">Disabled battle round.</param>
        /// <param name="targetPlayer">Player whose card effect is to be disabled.</param>
        internal void DisableRoundEffects(ushort round, PlayerAccount targetPlayer);

        /// <summary>
        /// Reserve a game action to be resolved.
        /// The first reserved action will be resolved last (resolved in stack-order).
        /// </summary>
        internal void PushToActionStack(IGameAction a);

        /// <summary>
        /// Set a Payer who wins by a card effect.
        /// </summary>
        /// <param name="p">Player to win.</param>
        internal void SetSpecialWinner(PlayerAccount p);

    }
}
