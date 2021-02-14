using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.Exceptions;
using OverroidModel.GameAction.Commands;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.GameAction.Commands
{
    public class MorphCommandTest
    {

        [Fact]
        public void Test_Validate_ValidWhenChosenFromOpponentPreviousCards()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInHumanHand: new List<CardName>() {CardName.Diva, CardName.Doctor, CardName.Beast }, // Defending first
                cardNamesInOverroidHand: new List<CardName>() { CardName.Creator, CardName.Hacker, CardName.Innocence } // Attacking first
            );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Creator, CardName.Diva, game);
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Doctor, CardName.Hacker, game);
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Innocence, CardName.Beast, game);

            var me = game.HumanPlayer;

            IGameCommand command = new MorphCommand(me, CardName.Hacker);
            command.Validate(game);
            Assert.True(true); // Not thrown
        }

        [Fact]
        public void Test_Validate_InvalidWhenChosenFromMyPreviousCards()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInHumanHand: new List<CardName>() {CardName.Diva, CardName.Doctor, CardName.Beast }, // Defending first
                cardNamesInOverroidHand: new List<CardName>() { CardName.Creator, CardName.Hacker, CardName.Innocence } // Attacking first
            );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Creator, CardName.Diva, game);
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Doctor, CardName.Hacker, game);
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Innocence, CardName.Beast, game);

            var me = game.HumanPlayer;

            IGameCommand command = new MorphCommand(me, CardName.Doctor);
            Assert.Throws<UnavailableActionException>(() => { command.Validate(game); });
        }

        [Fact]
        public void Test_Validate_InvalidWhenChosenFromOpponentCurrentCard()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInHumanHand: new List<CardName>() {CardName.Diva, CardName.Doctor, CardName.Beast }, // Defending first
                cardNamesInOverroidHand: new List<CardName>() { CardName.Creator, CardName.Hacker, CardName.Innocence } // Attacking first
            );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Creator, CardName.Diva, game);
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Doctor, CardName.Hacker, game);
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Innocence, CardName.Beast, game);

            var me = game.HumanPlayer;

            IGameCommand command = new MorphCommand(me, CardName.Innocence);
            Assert.Throws<UnavailableActionException>(() => { command.Validate(game); });
        }

        [Fact]
        public void Test_CreateRandomCommand_CreatedCommandIsValid()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInHumanHand: new List<CardName>() {CardName.Diva, CardName.Doctor, CardName.Beast }, // Defending first
                cardNamesInOverroidHand: new List<CardName>() { CardName.Creator, CardName.Hacker, CardName.Innocence } // Attacking first
            );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Creator, CardName.Diva, game);
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Doctor, CardName.Hacker, game);
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Innocence, CardName.Beast, game);

            var me = game.HumanPlayer;


            IGameCommand command = MorphCommand.CreateRandomCommand(game, me);
            command.Validate(game);
            Assert.True(true); // Not thrown
        }

        [Fact]
        public void Test_CreateRandomCommand_ThrowsWhenFirstRound()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInHumanHand: new List<CardName>() {CardName.Diva, CardName.Doctor, CardName.Beast }, // Defending first
                cardNamesInOverroidHand: new List<CardName>() { CardName.Creator, CardName.Hacker, CardName.Innocence } // Attacking first
            );

            Assert.Equal(1, game.CurrentBattle.Round);

            var me = game.HumanPlayer;

            Assert.Throws<UnavailableActionException>(() => { MorphCommand.CreateRandomCommand(game, me); });
        }
    }
}
