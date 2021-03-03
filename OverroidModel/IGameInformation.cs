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

        /// <summary>
        /// Player who plays Human force.
        /// </summary>
        public PlayerAccount HumanPlayer { get; }

        /// <summary>
        /// Player who plays Overroid force.
        /// </summary>
        public PlayerAccount OverroidPlayer { get; }

        /// <summary>
        /// Customized game rules.
        /// </summary>
        public IGameConfig Config { get; }

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
        /// All cards used in the game.
        /// </summary>
        public IReadOnlyList<InGameCard> AllInGameCards { get; }

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
        /// Whose and which command is expected to be given. If no command is expected, returns null.
        /// </summary>
        public ICommandRequirement? CommandRequirement { get; }

        /// <summary>
        /// Check if the game winner is already determined.
        /// </summary>
        public bool HasFinished();

        /// <summary>
        /// Get winner of the game if determined.
        /// </summary>
        public PlayerAccount? CheckWinner();

        /// <summary>
        /// Check if the game winner finished and is draw game.
        /// </summary>
        public bool IsDrawGame();

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
        /// Check if defending player can detect a card in given battle round.
        /// </summary>
        public bool DetectionIsAvailableInRound(ushort round);

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
        public bool EffectIsDisabled(ushort round, PlayerAccount p);
    }
}
