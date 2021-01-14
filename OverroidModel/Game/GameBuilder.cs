using OverroidModel.Card;
using OverroidModel.Card.Master;
using System;
using System.Linq;

namespace OverroidModel.Game
{
    /// <summary>
    /// Builder class to create IGameInformation implementations.
    /// </summary>
    class GameBuilder
    {
        readonly ICardMaster[] cardSet;

        /// <param name="cardSet">Card set to use with 11 cards that excludes Innocence and Overroid card.</param>
        public GameBuilder(ICardMaster[] cardSet)
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
        public IndividualGame InitializeIndividualGame(PlayerAccount humanPlayer, PlayerAccount overroidPlayer)
        {
            var shuffledCardSet = cardSet.OrderBy(c => Guid.NewGuid()).ToArray();
            
            var humanHand = shuffledCardSet[0..5].Select(c => new InGameCard(c)).ToList();
            var overroidHand = shuffledCardSet[5..11].Select(c => new InGameCard(c)).ToList();

            var miracleCard = new InGameCard(new Innocence());
            miracleCard.SetGuessed();
            humanHand.Add(miracleCard);

            var overroidCard = new InGameCard(new Overroid());
            overroidCard.SetGuessed();
            overroidHand.Add(overroidCard);

            return new IndividualGame(humanPlayer, overroidPlayer, new PlayerHand(humanHand), new PlayerHand(overroidHand));
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
