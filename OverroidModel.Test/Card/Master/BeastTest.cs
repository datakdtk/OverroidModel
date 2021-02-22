using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.GameAction.Commands;
using OverroidModel.GameAction.Effects;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.Card.Master
{
    public class BeastTest
    {
        [Fact]
        public void Test_CopiesBeforeBattle()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Beast } // Attacking
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Beast, CardName.Idol, game);

            Assert.False(game.Battles[0].HasFinished());
            CustomAssertion.WaitingForCommand<MorphCommand>(game.HumanPlayer, game);
        }

        [Fact]
        public void Test_DoesNotCopyOnFirstRound()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Beast }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Beast, CardName.Innocence, game);

            Assert.True(game.Battles[0].HasFinished());
            CustomAssertion.NotWaitingForCommand<MorphCommand>(game);
            CustomAssertion.ActionIsNotInHistory<CommandStandbyEffect<MorphCommand>>(game.ActionHistory);
            CustomAssertion.WinsInLastRound(CardName.Beast, game);
        }

        [Fact]
        public void Test_WisToOverroidWhenCopiesCreator()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Doctor, CardName.Beast }, // Attacking first
                cardNamesInHumanHand: new List<CardName>() { CardName.Creator, CardName.Overroid } // Defending first
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Doctor, CardName.Creator, game);
            Assert.Equal(2, game.CurrentBattle.Round);
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Overroid, CardName.Beast, game);

            var command = new MorphCommand(game.OverroidPlayer, CardName.Creator);
            game.ReceiveCommand(command);
            game.ResolveAllActions();

            CustomAssertion.WinsInLastRound(CardName.Beast, game);
        }

        [Fact]
        public void Test_WinsToCreatorWhenCopiesDoctor()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Innocence, CardName.Beast }, // Attacking first
                cardNamesInHumanHand: new List<CardName>() { CardName.Doctor, CardName.Creator } // Defending first
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Innocence, CardName.Doctor, game);
            Assert.Equal(2, game.CurrentBattle.Round);
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Creator, CardName.Beast, game);

            var command = new MorphCommand(game.OverroidPlayer, CardName.Doctor);
            game.ReceiveCommand(command);
            game.ResolveAllActions();

            CustomAssertion.WinsInLastRound(CardName.Beast, game);
        }

        [Fact]
        public void Test_SingularityWhenCopiesOverroidAndWins()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Doctor, CardName.Beast }, // Attacking first
                cardNamesInHumanHand: new List<CardName>() { CardName.Overroid, CardName.Innocence } // Defending first
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Doctor, CardName.Overroid, game);
            Assert.Equal(2, game.CurrentBattle.Round);
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Innocence, CardName.Beast, game);

            var command = new MorphCommand(game.OverroidPlayer, CardName.Overroid);
            game.ReceiveCommand(command);
            game.ResolveAllActions();

            CustomAssertion.WinsInLastRound(CardName.Beast, game);
            CustomAssertion.ActionIsInHistory<SingularityEffect>(game.ActionHistory);
            Assert.True(game.HasFinished());
        }

        [Fact]
        public void Test_NoSingularityWhenCopiesOverroidAndLost()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Doctor, CardName.Beast }, // Attacking first
                cardNamesInHumanHand: new List<CardName>() { CardName.Overroid, CardName.Creator } // Defending first
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Doctor, CardName.Overroid, game);
            Assert.Equal(2, game.CurrentBattle.Round);
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Creator, CardName.Beast, game);

            var command = new MorphCommand(game.OverroidPlayer, CardName.Overroid);
            game.ReceiveCommand(command);
            game.ResolveAllActions();

            CustomAssertion.LosesInLastRound(CardName.Beast, game);
            CustomAssertion.ActionIsNotInHistory<SingularityEffect>(game.ActionHistory);
            Assert.False(game.HasFinished());
        }

        [Fact]
        public void Test_WaitsForCommandWhenCopiesHacker()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Doctor, CardName.Beast }, // Attacking first
                cardNamesInHumanHand: new List<CardName>() { CardName.Hacker, CardName.Innocence } // Defending first
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Doctor, CardName.Hacker, game);
            Assert.Equal(2, game.CurrentBattle.Round);
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Innocence, CardName.Beast, game);

            var command = new MorphCommand(game.OverroidPlayer, CardName.Hacker);
            game.ReceiveCommand(command);
            game.ResolveAllActions();

            CustomAssertion.WinsInLastRound(CardName.Beast, game);
            CustomAssertion.WaitingForCommand<HackCommand>(game.OverroidPlayer, game);
            Assert.Equal(2, game.CurrentBattle.Round);
        }
    }
}
