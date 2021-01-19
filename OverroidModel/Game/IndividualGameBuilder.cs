using OverroidModel.Card.Master;
using OverroidModel.Config;
using OverroidModel.Game.Actions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OverroidModel.Game
{
    /// <summary>
    /// Builder class to create IGameInformation implementations.
    /// </summary>
    class IndividualGameBuilder
    {
        readonly ICardMaster[] cardSet;

        /// <param name="cardSet">Card set to use with 11 cards that excludes Innocence and Overroid card.</param>
        public IndividualGameBuilder(ICardMaster[] cardSet)
        {
            if (cardSet.Length != 11)
            {
                throw new ArgumentException("Length of card set must be 11");
            }
            this.cardSet = cardSet;
        }

        /// <summary>
        /// Create a new individual game.
        /// </summary>
        /// <param name="humanPlayer">Player who plays human force.</param>
        /// <param name="overroidPlayer">Player who plays overroid force.</param>
        /// <param name="config">Customized game rule.</param>
        public IndividualGame InitializeGame (
            PlayerAccount humanPlayer,
            PlayerAccount overroidPlayer,
            IGameConfig config)
        {
            var shuffledCardSet = cardSet.OrderBy(c => Guid.NewGuid()).ToList();
            var actions = new List<IGameAction>() { new RoundStart() };
            return LordGame(humanPlayer, overroidPlayer, shuffledCardSet, actions, config);
        }

        /// <summary>
        ///  Restore latest game state from initial state and history.
        /// </summary>
        /// <param name="humanPlayer">Player who plays human force.</param>
        /// <param name="overroidPlayer">Player who plays overroid force.</param>
        /// <param name="orderedDeck">Order of the deck at the beginning of the game</param>
        /// <param name="actionHistory">List of what has happened since game start,</param>
        /// <param name="config">Customized game rule.</param>
        public IndividualGame LordGame (
            PlayerAccount humanPlayer,
            PlayerAccount overroidPlayer,
            List<ICardMaster> orderedDeck,
            List<IGameAction> actionHistory,
            IGameConfig config
        )
        {
            var g = new IndividualGame(humanPlayer, overroidPlayer, orderedDeck, config);
            foreach (var a in actionHistory.Reverse<IGameAction>())
            {
                g.PushToActionStack(a);
            }
            g.ResolveStacks();
            return g;
        }

        /// <summary>
        /// Get cards of basic set.
        /// </summary>
        public static ICardMaster[] DefaultCardSet()
        {
            return new ICardMaster[11]
            {
                new Hacker(),
                new Creator(),
                new Doctor(),
                new Idol(),
                new Trickster(),
                new Spy(),
                new Diva(),
                new Beast(),
                new Legion(),
                new Soldier(),
                new Death(),
            };
        }
    }
}
