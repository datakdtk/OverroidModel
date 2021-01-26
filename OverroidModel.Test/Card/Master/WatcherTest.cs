using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.GameAction.Effects;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.Card.Master
{
    public class WatcherTest
    {
        [Fact]
        public void Test_TrigegersWhenLost()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Watcher }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Watcher, CardName.Innocence, game);

            CustomAssertion.LosesInLastRound(CardName.Watcher, game);
            CustomAssertion.ActionIsInHistory<PredictEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_GetAccessToHiddenCardByEffect()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Watcher }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
                );
            Assert.False(game.HiddenCard.IsViewableTo(game.OverroidPlayer));

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Watcher, CardName.Innocence, game);

            Assert.True(game.HiddenCard.IsViewableTo(game.OverroidPlayer));
            Assert.False(game.HiddenCard.IsViewableTo(game.HumanPlayer));
        }
    }
}
