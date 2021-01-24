using System;
using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.GameAction.Commands;
using OverroidModel.GameAction.Effects;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.Card.Master
{
    public class CreatorTest
    {

        [Fact]
        public void Test_WinsToInnocence()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Creator }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Creator, CardName.Innocence, game);

            CustomAssertion.WinsInLastRound(CardName.Creator, game);
            CustomAssertion.ActionIsNotInHistory<LifemakerEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_LosesToInnocence()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Creator }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Idol } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Creator, CardName.Idol, game);

            CustomAssertion.LosesInLastRound(CardName.Creator, game);
            CustomAssertion.ActionIsNotInHistory<LifemakerEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_WinsToBeastByEffect()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Creator }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Beast } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Creator, CardName.Beast, game);

            // Expected Morph not to be triggered because it is first round.
            if (game.ExpectedCommandInfo?.type == typeof(MorphCommand))
            {
                throw new Exception("battle has not finished because waiting for morph command");
            }

            CustomAssertion.WinsInLastRound(CardName.Creator, game);
            CustomAssertion.ActionIsInHistory<LifemakerEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_WinsToOverroidByEffect()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Creator }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Overroid } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Creator, CardName.Overroid, game);

            CustomAssertion.WinsInLastRound(CardName.Creator, game);
            CustomAssertion.ActionIsInHistory<LifemakerEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_GameNotFinishAfterBattleWithOverroid()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Creator }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Overroid } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Creator, CardName.Overroid, game);

            Assert.False(game.HasFinished());
            CustomAssertion.ActionIsNotInHistory<SingularityEffect>(game.ActionHistory);
        }
    }
}
