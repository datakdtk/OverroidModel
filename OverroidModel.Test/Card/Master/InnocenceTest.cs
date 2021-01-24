using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.Game.Actions;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.Card.Master
{
    public class InnocenceTest
    {

        [Fact]
        public void Test_LosesToCreator()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Creator }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Creator, CardName.Innocence, game);

            CustomAssertion.LosesInLastRound(CardName.Innocence, game);
            CustomAssertion.ActionIsNotInHistory<MiracleEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_LosesToIdol()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Idol, CardName.Innocence, game);

            CustomAssertion.LosesInLastRound(CardName.Innocence, game);
            CustomAssertion.ActionIsNotInHistory<MiracleEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_LosesToDiva()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Diva, CardName.Innocence, game);

            CustomAssertion.LosesInLastRound(CardName.Innocence, game);
            CustomAssertion.ActionIsNotInHistory<MiracleEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_WinsToOverroid()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Overroid }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Overroid, CardName.Innocence, game);

            CustomAssertion.WinsInLastRound(CardName.Innocence, game);
            CustomAssertion.ActionIsInHistory<MiracleEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_GameFinishesAfterBattleWithOverroid()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Overroid }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Overroid, CardName.Innocence, game);

            Assert.True(game.HasFinished());
            Assert.Equal(game.HumanPlayer, game.Winner);
            Assert.Equal(1, game.Battles.Count);
            CustomAssertion.ActionIsNotInHistory<SingularityEffect>(game.ActionHistory);
        }
    }
}
