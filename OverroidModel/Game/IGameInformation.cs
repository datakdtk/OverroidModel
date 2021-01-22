using OverroidModel.Exceptions;
using OverroidModel.Game.Actions;
using OverroidModel.Game.Actions.Commands;
using System.Collections.Generic;

namespace OverroidModel.Game
{
    /// <summary>
    /// Common interface for taking all kinds of games.
    /// </summary>
    public interface IGameInformation
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
        public IReadOnlyList<IGameAction> ActionHistory { get; }

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
        /// Whether defending player detect a card in battles.
        /// </summary>
        public bool DetectionAvailable { get; }

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

    }
}
