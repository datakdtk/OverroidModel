using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.Exceptions;
using OverroidModel.GameAction.Commands;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.GameAction.Commands
{
    public class EspionageCommandTest
    {

        [Fact]
        public void Test_Validate_ValidWhenBothPlayerHasTargetCards()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 4,
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence },
                cardNamesInOverroidHand: new List<CardName>() { CardName.Overroid }
            );
            var me = game.OverroidPlayer;

            IGameCommand command = new EspionageCommand(me, CardName.Overroid, CardName.Innocence);
            command.Validate(game);
            Assert.True(true); // Not thrown
        }

        [Fact]
        public void Test_Validate_ValidWhenTargetCardsAreRevealed()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 4,
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence },
                cardNamesInOverroidHand: new List<CardName>() { CardName.Overroid }
            );
            var me = game.OverroidPlayer;
            game.HandOf(me).CardOf(CardName.Overroid)!.RevealByHack();
            game.HandOf(game.OpponentOf(me)).CardOf(CardName.Innocence)!.RevealByHack();

            IGameCommand command = new EspionageCommand(me, CardName.Overroid, CardName.Innocence);
            command.Validate(game);
            Assert.True(true); // Not thrown
        }

        [Fact]
        public void Test_Validate_InvalidWhenTargetCardsOrerIsWrong()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 4,
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence },
                cardNamesInOverroidHand: new List<CardName>() { CardName.Overroid }
            );
            var me = game.OverroidPlayer;

            IGameCommand command = new EspionageCommand(me, CardName.Innocence, CardName.Overroid);
            Assert.Throws<UnavailableActionException>( () => {
                command.Validate(game);
            });
        }

        [Fact]
        public void Test_CreateRandomCommand_CreatedCommandIsValid()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 4,
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence },
                cardNamesInOverroidHand: new List<CardName>() { CardName.Overroid }
            );
            var me = game.OverroidPlayer;

            IGameCommand command = EspionageCommand.CreateRandomCommand(game, me);
            command.Validate(game);
            Assert.True(true); // Not thrown
        }

        [Fact]
        public void Test_CreateRandomCommand_ThrownWhenHandIsEmpty()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 6, // final round
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence },
                cardNamesInOverroidHand: new List<CardName>() { CardName.Spy }
            );
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Innocence, CardName.Spy, game); // make hands empty
            var me = game.OverroidPlayer;

            Assert.Equal(0, game.HandOf(me).Count);

            Assert.Throws<UnavailableActionException>( () => {
                EspionageCommand.CreateRandomCommand(game, me);
            });
        }

        [Fact]
        public void Test_CreateRandomCommand_NotThrowsWhenRevealedCardOnly()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 4,
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence, CardName.Idol },
                cardNamesInOverroidHand: new List<CardName>() { CardName.Overroid }
            );
            var me = game.OverroidPlayer;
            var opponentHand = game.HandOf(game.OpponentOf(me));
            opponentHand.CardOf(CardName.Innocence)!.RevealByHack();
            opponentHand.CardOf(CardName.Idol)!.RevealByHack();

            EspionageCommand.CreateRandomCommandWithMyCardChoice(game, me, CardName.Unknown);
            Assert.True(true); // Not thrown
        }

        [Fact]
        public void Test_CreateRandomCommandWithMyCardChoice_CreatedCommandIsValid()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 4,
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence, CardName.Idol }
            );
            var me = game.OverroidPlayer;
            game.HandOf(game.OpponentOf(me)).CardOf(CardName.Innocence)!.RevealByHack();

            IGameCommand command = EspionageCommand.CreateRandomCommandWithMyCardChoice(game, game.OverroidPlayer, CardName.Unknown);
            command.Validate(game);
            Assert.True(true); // Not thrown
        }

        [Fact]
        public void Test_CreateRandomCommandWithMyCardChoice_ChoosesUnRevealedCard()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 4,
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence, CardName.Idol }
            );
            var me = game.OverroidPlayer;
            game.HandOf(game.OpponentOf(me)).CardOf(CardName.Innocence)!.RevealByHack();

            var command = EspionageCommand.CreateRandomCommandWithMyCardChoice(game, me, CardName.Unknown);
            Assert.Equal(CardName.Idol, command.SecondTargetCardName);
        }

        [Fact]
        public void Test_CreateRandomCommandWithMyCardChoice_ThrowsWhenRevealedCardOnly()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 4,
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence, CardName.Idol }
            );
            var me = game.OverroidPlayer;
            var opponentHand = game.HandOf(game.OpponentOf(me));
            foreach (var c in opponentHand.Cards)
            {
                c.RevealByHack();
            }

            Assert.Throws<UnavailableActionException>( () => {
                EspionageCommand.CreateRandomCommandWithMyCardChoice(game, me, CardName.Unknown);
            });
        }

        [Fact]
        public void Test_CreateRandomCommand_ThrownWhenCommandingPlayerDoesNotHaveTargetCard()
        {
            var game = TestGameBuilder.CreateIndividualGame();
            var me = game.OverroidPlayer;

            Assert.Throws<CardNotFoundException>( () => {
                EspionageCommand.CreateRandomCommandWithMyCardChoice(game, me, CardName.Innocence);
            });
        }
    }
}
