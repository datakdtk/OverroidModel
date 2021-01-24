using OverroidModel.Card;
using OverroidModel.Config;
using OverroidModel.Game.Actions;
using OverroidModel.Game.Actions.Commands;
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
        readonly ICardShuffler shuffler;

        /// <param name="shuffler">Determines how to shuffle initial card deck.</param>
        public IndividualGameBuilder(ICardShuffler shuffler)
        {
            this.shuffler = shuffler;
        }

        /// <summary>
        /// Create a new individual game.
        /// </summary>
        /// <param name="humanPlayer">Player who plays human force.</param>
        /// <param name="overroidPlayer">Player who plays overroid force.</param>
        /// <param name="shufflingSeed">Randomizing seed to determine order of card deck.</param>
        /// <param name="config">Customized game rule.</param>
        public IndividualGame InitializeGame (
            PlayerAccount humanPlayer,
            PlayerAccount overroidPlayer,
            string shufflingSeed,
            IGameConfig config
        )
        {
            var cardSet = CardDictionary.DefaultCardList;
            var shuffledDeck = shuffler.Shuffle(cardSet, shufflingSeed);

            var humanHand = new PlayerHand(shuffledDeck.GetRange(0, 5).Select(c => new InGameCard(c)));
            var overroidHand = new PlayerHand(shuffledDeck.GetRange(5, 5).Select(c => new InGameCard(c)));
            humanHand.AddCard(CardDictionary.GetInGameCard(CardName.Innocence));
            overroidHand.AddCard(CardDictionary.GetInGameCard(CardName.Overroid));

            var game = new IndividualGame(humanPlayer, overroidPlayer, humanHand, overroidHand, config);
            game.PushToActionStack(new RoundStart());
            game.ResolveStacks();
            return game;
        }

        /// <summary>
        ///  Restore latest game state from initial state and commands.
        /// </summary>
        /// <param name="humanPlayer">Player who plays human force.</param>
        /// <param name="overroidPlayer">Player who plays overroid force.</param>
        /// <param name="shufflingSeed">Randomizing seed to determine order of card deck.</param>
        /// <param name="config">Customized game rule.</param>
        /// <param name="commandHistory">History of the choices of the players from beginning of the game.</param>
        /// <exception cref="Exceptions.UnavailableActionException">Thrown when commandHistory is invalid.</exception>
        public IndividualGame LordGame (
            PlayerAccount humanPlayer,
            PlayerAccount overroidPlayer,
            string shufflingSeed,
            IGameConfig config,
            IEnumerable<IGameCommand> commandHistory
        )
        {
            var game = InitializeGame(humanPlayer, overroidPlayer, shufflingSeed, config);
            foreach (var c in commandHistory)
            {
                game.ReceiveCommand(c);
            }
            return game;
        }
    }
}
