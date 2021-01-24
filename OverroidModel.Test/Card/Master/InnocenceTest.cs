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
                cardNamesInHumanHand: new List<CardName>() { CardName.Inocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Creator, CardName.Inocence, game);

            CustomAssertion.LosesInLastRound(CardName.Inocence, game);
            CustomAssertion.ActionIsNotInHistory<MiracleEffect>(game.ActionHistory);
            CustomAssertion.ActionIsNotInHistory<LifemakerEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_LosesToIdol()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Inocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Idol, CardName.Inocence, game);

            CustomAssertion.LosesInLastRound(CardName.Inocence, game);
            CustomAssertion.ActionIsNotInHistory<MiracleEffect>(game.ActionHistory);
            CustomAssertion.ActionIsNotInHistory<CharmEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_WinsToTrickster()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Trickster }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Inocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Trickster, CardName.Inocence, game);

            CustomAssertion.WinsInLastRound(CardName.Inocence, game);
            CustomAssertion.ActionIsNotInHistory<MiracleEffect>(game.ActionHistory);
            CustomAssertion.ActionIsInHistory<ReversalEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_LosesToDiva()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Inocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Diva, CardName.Inocence, game);

            CustomAssertion.LosesInLastRound(CardName.Inocence, game);
            CustomAssertion.ActionIsNotInHistory<MiracleEffect>(game.ActionHistory);
            CustomAssertion.ActionIsNotInHistory<InspirationEfect>(game.ActionHistory);
        }

        [Fact]
        public void Test_WinsToOverroid()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Overroid }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Inocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Overroid, CardName.Inocence, game);

            CustomAssertion.WinsInLastRound(CardName.Inocence, game);
            CustomAssertion.ActionIsInHistory<MiracleEffect>(game.ActionHistory);
            CustomAssertion.ActionIsNotInHistory<InspirationEfect>(game.ActionHistory);
        }

        [Fact]
        public void Test_GameFinishesAfterBattleWithOverroid()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Overroid }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Inocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Overroid, CardName.Inocence, game);

            Assert.True(game.HasFinished());
            Assert.Equal(game.HumanPlayer, game.Winner);
            Assert.Equal(1, game.Battles.Count);
        }
    }
}
