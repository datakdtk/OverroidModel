using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.GameAction.Commands;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.Card.Master
{
    public class HackerTest
    {

        [Fact]
        public void Test_HackTrigegersWhenWon()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Hacker }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Hacker, CardName.Innocence, game);

            CustomAssertion.WinsInLastRound(CardName.Hacker, game);
            CustomAssertion.WaitingForCommand<HackCommand>(game.OverroidPlayer, game);
        }

        [Fact]
        public void Test_HackTrigegersWhenLost()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Hacker }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Diva } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Hacker, CardName.Diva, game);

            CustomAssertion.LosesInLastRound(CardName.Hacker, game);
            CustomAssertion.WaitingForCommand<HackCommand>(game.OverroidPlayer, game);
        }

        [Fact]
        public void Test_HackDoesNotTrigegerWhenOpponentHasNoHand()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 6,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Hacker }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Attacking
                );
            Assert.Equal(1, game.HandOf(game.HumanPlayer).Count);

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Innocence, CardName.Hacker, game);

            CustomAssertion.WinsInLastRound(CardName.Hacker, game);
            CustomAssertion.NotWaitingForCommand<HackCommand>(game);
        }

        [Fact]
        public void Test_HackDoesNotTrigegerWhenOpponentHasNoHiddenCard()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 5,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Hacker }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence, CardName.Creator } // Defending
                );
            game.HandOf(game.HumanPlayer).CardOf(CardName.Creator)!.RevealByHack();

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Hacker, CardName.Innocence, game);

            CustomAssertion.WinsInLastRound(CardName.Hacker, game);
            CustomAssertion.CardIsHacked(game.HandOf(game.HumanPlayer).Cards[0]);
            CustomAssertion.NotWaitingForCommand<HackCommand>(game);
        }

        [Fact]
        public void Test_HackTargetCardGetsRevealed()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Hacker }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence, CardName.Creator } // Defending
                );
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Hacker, CardName.Innocence, game);

            CustomAssertion.WaitingForCommand<HackCommand>(game.OverroidPlayer, game);

            var command = new HackCommand(game.OverroidPlayer, CardName.Creator);
            game.ReceiveCommand(command);

            var hand = game.HandOf(game.HumanPlayer);
            CustomAssertion.CardIsHacked(hand.CardOf(CardName.Creator)!);
            Assert.Equal(2, game.Battles.Count); // New round has begun,
        }

        [Fact]
        public void Test_CommandGenerateChoosesUnRevealedCard()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 4,
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence, CardName.Idol } // Defending
                );
            var hand = game.HandOf(game.HumanPlayer);
            hand.CardOf(CardName.Innocence)!.RevealByHack();

            var command = CommandGenerate.CreateHackCommand(game, game.OverroidPlayer);
            Assert.Equal(CardName.Idol, command.TargetCardName);


        }
    }
}
