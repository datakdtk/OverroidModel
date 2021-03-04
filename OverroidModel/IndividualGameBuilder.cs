using OverroidModel.Card;
using OverroidModel.GameAction;
using OverroidModel.GameAction.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OverroidModel
{
    /// <summary>
    /// Builder class to create IGameInformation implementations.
    /// </summary>
    public class IndividualGameBuilder
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
        public IndividualGame InitializeGame(
            PlayerAccount humanPlayer,
            PlayerAccount overroidPlayer,
            int shufflingSeed,
            IGameConfig config
        )
        {
            var cardSet = config.UsesWatcher
                ? CardDictionary.DefaultCardListPlusWatcher
                : CardDictionary.DefaultCardList;
            var shuffledDeck = shuffler.Shuffle(cardSet, shufflingSeed);

            var humanCards = shuffledDeck.GetRange(0, 5).Select(c => new InGameCard(c, humanPlayer));
            var humanHand = new PlayerHand(humanPlayer, humanCards);
            humanHand.AddCard(CardDictionary.GetInGameCard(CardName.Innocence, humanPlayer));

            var overroidCards = shuffledDeck.GetRange(5, 5).Select(c => new InGameCard(c, overroidPlayer));
            var overroidHand = new PlayerHand(overroidPlayer, overroidCards);
            overroidHand.AddCard(CardDictionary.GetInGameCard(CardName.Overroid, overroidPlayer));

            var hiddenCard = shuffledDeck[10];
            var triggerCard = shuffledDeck.Count >= 12 ? shuffledDeck[11] : null;

            var game = new IndividualGame(
                humanPlayer,
                overroidPlayer,
                humanHand,
                overroidHand,
                hiddenCard,
                triggerCard,
                config
            );
            game.PushToActionStack(new RoundStart());
            game.PushToActionStack(new GameStart());
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
        public IndividualGame LordGame(
            PlayerAccount humanPlayer,
            PlayerAccount overroidPlayer,
            int shufflingSeed,
            IGameConfig config,
            IEnumerable<IGameCommand> commandHistory
        )
        {
            var game = InitializeGame(humanPlayer, overroidPlayer, shufflingSeed, config);
            foreach (var c in commandHistory)
            {
                game.ReceiveCommand(c);
                game.ResolveAllActions();
            }
            return game;
        }
    }
}
