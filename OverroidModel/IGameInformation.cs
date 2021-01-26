using OverroidModel.Card;
using OverroidModel.Exceptions;
using OverroidModel.GameAction;
using System;
using System.Collections.Generic;

namespace OverroidModel
{
    /// <summary>
    /// Common interface for taking all kinds of games.
    /// </summary>
    public interface IGameInformation
    {
        const ushort maxRound = 6;

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
        /// Card that was not distributed to player's hands.
        /// </summary>
        public OutsideCard HiddenCard { get;  }

        /// <summary>
        /// Another card that was not distributed to player's hands.
        /// Set only when card set includes Watcher card. Otherwise, always null.
        /// </summary>
        public OutsideCard? TriggerCard { get;  }

        /// <summary>
        /// Winner of the game if already determined.
        /// </summary>
        public PlayerAccount? Winner { get; }

        /// <summary>
        /// Whether defending player detect a card in battles.
        /// </summary>
        public bool DetectionAvailable { get; }

        /// <summary>
        /// Tells whose and which command is expected to be given. If no command is expected, returns null.
        /// </summary>
        public (Type type, PlayerAccount player)? ExpectedCommandInfo { get; }

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
        /// Check if card effect will be triggered normally.
        /// </summary>
        /// <returns>
        /// Returns true if card effect of given player is disabled in given round.
        /// If player is null, returns false.
        /// </returns>
        public bool EffectIsDisabled(ushort round, PlayerAccount? p);
    }
}
