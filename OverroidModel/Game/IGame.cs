using OverroidModel.Exceptions;
using OverroidModel.Game.Actions;
using OverroidModel.Game.Actions.Commands;
using System.Collections.Generic;

namespace OverroidModel.Game
{
    /// <summary>
    /// Common interface of all kinds of games.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Player who plays Human force.
        /// </summary>
        public PlayerAccount HumanPlayer { get; }

        /// <summary>
        /// Player who plays Overroid force.
        /// </summary>
        public PlayerAccount OverroidPlayer { get; }

        /// <summary>
        /// Game actions from the beginning to current game state.
        /// </summary>
        public List<IGameAction> ActionHistory { get; }

        /// <summary>
        /// List of battles in this game.
        /// </summary>
        public IReadOnlyList<Battle> Battles { get; }

        /// <summary>
        /// Battle of current round.
        /// </summary>
        public Battle CurrentBattle { get; }

        /// <summary>
        /// Winner of the game if already determined.
        /// </summary>
        public PlayerAccount? Winner{ get; }

        /// <summary>
        /// Check if the game winner is already determined.
        /// </summary>
        public bool HasFinished();

        /// <summary>
        /// Check if the game winner finished and is draw game.
        /// </summary>
        public bool IsDrawGame() => HasFinished() && Winner == null;

        /// <summary>
        /// Get opponent player of given player.
        /// </summary>
        /// <param name="p">Player in the game.</param>
        /// <exception cref="NonGamePlayerException"></exception>
        public PlayerAccount OpponentOf(PlayerAccount p);

        /// <summary>
        /// Get hand cards of given player.
        /// </summary>
        /// <param name="p">Player in the game.</param>
        /// <exception cref="NonGamePlayerException"></exception>
        public PlayerHand HandOf(PlayerAccount p);

        /// <summary>
        /// Get hand cards of given player.
        /// </summary>
        /// <param name="p">Player in the game.</param>
        /// <exception cref="NonGamePlayerException"></exception>
        public ushort WinningStarOf(PlayerAccount p);

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
        /// <param name="targetPlayer">Player whose card effect is to be disabled.</param>
        /// <param name="round">Disabled battle round.</param>
        internal void DisableRoundEffects(PlayerAccount targetPlayer, ushort round);

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
