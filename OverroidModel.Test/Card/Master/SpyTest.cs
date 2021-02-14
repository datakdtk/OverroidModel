using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.GameAction.Commands;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.Card.Master
{
    public class SpyTest
    {

        [Fact]
        public void Test_TrigegersWhenWon()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Spy }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
            );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Spy, CardName.Innocence, game);

            CustomAssertion.WinsInLastRound(CardName.Spy, game);
            CustomAssertion.WaitingForCommand<EspionageCommand>(game.OverroidPlayer, game);
        }

        [Fact]
        public void Test_TrigegersWhenLost()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Spy }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Diva } // Defending
            );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Spy, CardName.Diva, game);

            CustomAssertion.LosesInLastRound(CardName.Spy, game);
            CustomAssertion.WaitingForCommand<EspionageCommand>(game.OverroidPlayer, game);
        }

        [Fact]
        public void Test_DoesNotTrigegerWhenOpponentHasNoHand()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 6,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Spy }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Attacking
            );
            Assert.Equal(1, game.HandOf(game.HumanPlayer).Count);

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Innocence, CardName.Spy, game);

            CustomAssertion.WinsInLastRound(CardName.Spy, game);
            CustomAssertion.NotWaitingForCommand<EspionageCommand>(game);
        }

        [Fact]
        public void Test_TrigegersWhenOpponentHasNoHiddenCard()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 5,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Spy }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence, CardName.Creator } // Defending
            );
            game.HandOf(game.HumanPlayer).CardOf(CardName.Creator)!.RevealByHack();

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Spy, CardName.Innocence, game);

            CustomAssertion.WinsInLastRound(CardName.Spy, game);
            CustomAssertion.CardIsHacked(game.HandOf(game.HumanPlayer).Cards[0]);
            CustomAssertion.WaitingForCommand<EspionageCommand>(game.OverroidPlayer, game);
        }

        [Fact]
        public void Test_TargetCardsGetExchanged()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol, CardName.Spy }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence, CardName.Creator } // Defending
            );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Spy, CardName.Innocence, game);

            CustomAssertion.WaitingForCommand<EspionageCommand>(game.OverroidPlayer, game);

            var command = new EspionageCommand(game.OverroidPlayer, CardName.Idol, CardName.Creator);
            game.ReceiveCommand(command);


            Assert.True(game.HandOf(game.HumanPlayer).HasCard(CardName.Idol));
            Assert.True(game.HandOf(game.OverroidPlayer).HasCard(CardName.Creator));
            Assert.Equal(2, game.Battles.Count); // New round has begun,
        }

        [Fact]
        public void Test_HackedCardIsStillHackedAfterExchange()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol, CardName.Spy }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence, CardName.Creator } // Defending
            );

            game.HandOf(game.OverroidPlayer).CardOf(CardName.Idol)!.RevealByHack();

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Spy, CardName.Innocence, game);

            CustomAssertion.WaitingForCommand<EspionageCommand>(game.OverroidPlayer, game);

            var command = new EspionageCommand(game.OverroidPlayer, CardName.Idol, CardName.Creator);
            game.ReceiveCommand(command);

            Assert.True(game.HandOf(game.HumanPlayer).HasCard(CardName.Idol));
            CustomAssertion.CardIsHacked(game.HandOf(game.HumanPlayer).CardOf(CardName.Idol)!);
        }
    }
}
