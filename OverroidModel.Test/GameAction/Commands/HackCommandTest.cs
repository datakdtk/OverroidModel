using System;
using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.Exceptions;
using OverroidModel.GameAction.Commands;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.GameAction.Commands
{
    public class HackCommandTest
    {

        [Fact]
        public void Test_Validate_NotThrowsWhenChosenFromOpponentHand()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInHumanHand: new List<CardName>() { CardName.Creator}
            );
            var me = game.OverroidPlayer;
            IGameCommand command = new HackCommand(me, CardName.Creator);
            command.Validate(game);
            Assert.True(true); // Not thrown;
        }

        [Fact]
        public void Test_Validate_ThrowsWhenNotChosenFromMyHand()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Creator }
            );
            var me = game.OverroidPlayer;
            IGameCommand command = new HackCommand(me, CardName.Creator);
            Assert.Throws<UnavailableActionException>(() => { command.Validate(game); });
        }

        [Fact]
        public void Test_CreateRandomCommand_CreatedCommandIsValid()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 4,
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence, CardName.Idol }
            );
            var me = game.OverroidPlayer;
            game.HandOf(game.HumanPlayer).CardOf(CardName.Innocence)!.RevealByHack();

            IGameCommand command = HackCommand.CreateRandomCommand(game, me);
            command.Validate(game);
            Assert.True(true); // Not thrown;
        }

        [Fact]
        public void Test_CreateRandomCommand_ChoosesNonhackedCard()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 4,
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence, CardName.Idol }
            );
            var me = game.OverroidPlayer;
            game.HandOf(game.OpponentOf(me)).CardOf(CardName.Innocence)!.RevealByHack();

            var command = HackCommand.CreateRandomCommand(game, me);
            Assert.Equal(CardName.Idol, command.TargetCardName);
        }
    }
}
