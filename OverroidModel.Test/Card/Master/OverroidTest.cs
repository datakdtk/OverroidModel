using System;
using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.GameAction.Commands;
using OverroidModel.GameAction.Effects;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.Card.Master
{
    public class OverroidTest
    {
        [Fact]
        public void Test_SingularityWhenWon()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Overroid }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Idol } // Attacking
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Idol, CardName.Overroid, game);

            CustomAssertion.WinsInLastRound(CardName.Overroid, game);
            CustomAssertion.ActionIsInHistory<SingularityEffect>(game.ActionHistory);
            Assert.True(game.HasFinished());
            Assert.Equal(game.OverroidPlayer, game.CheckWinner());
        }

        [Fact]
        public void Test_NoSingularityWhenLost()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Overroid } // Defending
                );

            if (game.HandOf(game.OverroidPlayer).CardOf(CardName.Unknown)?.Value! <= 12)
            {
                throw new Exception("Value of dummy card is not strong enough");
            }

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Unknown, CardName.Overroid, game);


            CustomAssertion.LosesInLastRound(CardName.Overroid, game);
            CustomAssertion.ActionIsNotInHistory<SingularityEffect>(game.ActionHistory);
            Assert.False(game.HasFinished());
        }

        [Fact]
        public void Test_HackDoesNotTriggerWhenSingularity()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Overroid }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Hacker } // Attacking
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Hacker, CardName.Overroid, game);

            CustomAssertion.WinsInLastRound(CardName.Overroid, game);
            CustomAssertion.ActionIsInHistory<SingularityEffect>(game.ActionHistory);
            CustomAssertion.ActionIsNotInHistory<CommandStandbyEffect<HackCommand>>(game.ActionHistory);
            CustomAssertion.NotWaitingForCommand<HackCommand>(game);
        }

        [Fact]
        public void Test_EspionageDoesNotTriggerWhenSingularity()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Overroid }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Spy } // Attacking
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Spy, CardName.Overroid, game);

            CustomAssertion.WinsInLastRound(CardName.Overroid, game);
            CustomAssertion.ActionIsInHistory<SingularityEffect>(game.ActionHistory);
            CustomAssertion.ActionIsNotInHistory<CommandStandbyEffect<EspionageCommand>>(game.ActionHistory);
            CustomAssertion.NotWaitingForCommand<EspionageCommand>(game);

        }
    }
}
