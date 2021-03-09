using System.Collections.Generic;
using System.Linq;
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
            Assert.False(game.HiddenCard.IsVisibleTo(game.OverroidPlayer));

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Watcher, CardName.Innocence, game);

            Assert.True(game.HiddenCard.IsVisibleTo(game.OverroidPlayer));
            Assert.False(game.HiddenCard.IsVisibleTo(game.HumanPlayer));
        }

        [Fact]
        public void Test_MakeAllOpponentHandCardsGuessableByEffect()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Watcher }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
            );
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Watcher, CardName.Innocence, game);

            var opponentHand = game.HandOf(game.HumanPlayer);
            Assert.True(opponentHand.Cards.All(c => c.IsGuessable()));
        }

        [Fact]
        public void Test_HackedOpponentHandIsStillHackedAfterPredictEffect()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Watcher }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence, CardName.Creator } // Defending
            );
            var opponentHand = game.HandOf(game.HumanPlayer);
            InGameCard hackedCard = opponentHand.CardOf(CardName.Creator)!;
            hackedCard.RevealByHack();
            Assert.True(hackedCard.IsHacked());

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Watcher, CardName.Innocence, game);
            Assert.True(hackedCard.IsHacked());
        }
    }
}
