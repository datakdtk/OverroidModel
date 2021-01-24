using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.Game.Actions;
using OverroidModel.Game.Actions.Commands;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.Card.Master
{
    public class IdolTest
    {

        [Fact]
        public void Test_WinsToInnocence()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Idol, CardName.Innocence, game);

            CustomAssertion.WinsInLastRound(CardName.Idol, game);
            CustomAssertion.ActionIsNotInHistory<CharmEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_LosesToDiva()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Diva } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Idol, CardName.Diva, game);

            CustomAssertion.LosesInLastRound(CardName.Idol, game);
            CustomAssertion.ActionIsNotInHistory<CharmEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_WinsToSoldierByEffect()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Soldier } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Idol, CardName.Soldier, game);

            CustomAssertion.WinsInLastRound(CardName.Idol, game);
            CustomAssertion.ActionIsInHistory<CharmEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_RushIsNotTriggeredAfterBattle()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Soldier } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Idol, CardName.Soldier, game);

            var expectedCommand = game.ExpectedCommandInfo;
            Assert.NotEqual(typeof(RushCommand), expectedCommand?.type);
            CustomAssertion.ActionIsNotInHistory<RushCommand>(game.ActionHistory);
        }
    }
}
