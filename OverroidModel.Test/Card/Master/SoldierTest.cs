using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.GameAction.Commands;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.Card.Master
{
    public class SoldierTest
    {
        [Fact]
        public void Test_TrigegersWhenWon()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Soldier }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Soldier, CardName.Innocence, game);

            CustomAssertion.WinsInLastRound(CardName.Soldier, game);
            CustomAssertion.WaitingForCommand<RushCommand>(game.OverroidPlayer, game);
        }

        [Fact]
        public void Test_DoesNotTrigegerWhenLost()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Soldier }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Diva } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Soldier, CardName.Diva, game);

            CustomAssertion.LosesInLastRound(CardName.Soldier, game);
            CustomAssertion.NotWaitingForCommand<RushCommand>(game);
        }

        [Fact]
        public void Test_DoesNotTrigegerOnLastRound()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 6,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Soldier }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Attacking
                );
            Assert.Equal(1, game.HandOf(game.HumanPlayer).Count);

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Innocence, CardName.Soldier, game);

            CustomAssertion.WinsInLastRound(CardName.Soldier, game);
            CustomAssertion.NotWaitingForCommand<RushCommand>(game);
        }

        [Fact]
        public void Test_TargetCardsGetExchanged()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol, CardName.Soldier }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Soldier, CardName.Innocence, game);

            CustomAssertion.WaitingForCommand<RushCommand>(game.OverroidPlayer, game);

            var command = new RushCommand(game.OverroidPlayer, CardName.Idol);
            game.ReceiveCommand(command);


            Assert.Equal(CardName.Idol, game.Battles[0].CardOf(game.OverroidPlayer).Name);
            Assert.True(game.HandOf(game.OverroidPlayer).HasCard(CardName.Soldier));

            Assert.Equal(2, game.Battles.Count); // New round has begun,
        }


        [Fact]
        public void Test_WinnerDoesNotChangeAfterExchanging()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol, CardName.Soldier }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Soldier, CardName.Innocence, game);

            CustomAssertion.WaitingForCommand<RushCommand>(game.OverroidPlayer, game);

            var command = new RushCommand(game.OverroidPlayer, CardName.Idol);
            game.ReceiveCommand(command);

            Assert.Equal(game.OverroidPlayer, game.Battles[0].Winner);
        }
    }
}
