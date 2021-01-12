using OverroidModel.Card;
using OverroidModel.Card.Master;
using System;
using System.Linq;

namespace OverroidModel.Game
{
    class GameBuilder
    {
        readonly ICardMaster[] cardSet;

        public GameBuilder(ICardMaster[] cardSet)
        {
            if (cardSet.Length != 11)
            {
                throw new ArgumentException("Length of card set must be 11");
            }
            this.cardSet = cardSet;
        }

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
