using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.Exceptions;
using OverroidModel.GameAction.Commands;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.GameAction.Commands
{
    public class CardPlacementTest
    {

        [Fact]
        public void Test_PlacedCardNameIsNotTargetCardNAme()
        {
            var command = new CardPlacement(new PlayerAccount("hoge"), CardName.Innocence);
            Assert.Null(command.TargetCardName);
            Assert.Null(command.SecondTargetCardName);
        }

        [Fact]
        public void Test_Validate_NotThrowsWhenChosenFromMyHand()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Creator}
            );
            var me = game.OverroidPlayer;
            IGameCommand command = new CardPlacement(me, CardName.Creator);
            command.Validate(game);
            Assert.True(true); // Not thrown;
        }

        [Fact]
        public void Test_Validate_ThrowsWhenNotChosenFromOpponentHand()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInHumanHand: new List<CardName>() { CardName.Creator }
            );
            var me = game.OverroidPlayer;
            IGameCommand command = new CardPlacement(me, CardName.Creator);
            Assert.Throws<UnavailableActionException>(() => { command.Validate(game); });
        }

        [Fact]
        public void Test_CreateRandomCommand_CreatedCommandIsValid()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Innocence, CardName.Idol }
            );
            var me = game.OverroidPlayer;

            IGameCommand command = CardPlacement.CreateRandomCommand(game, me);
            command.Validate(game);
            Assert.True(true); // Not thrown;
        }

        [Fact]
        public void Test_CreateRandomCommand_ThrownWhenHandIsEmpty()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 6, // final round
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence },
                cardNamesInOverroidHand: new List<CardName>() { CardName.Soldier }
            );
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Innocence, CardName.Soldier, game); // make hands empty
            var me = game.OverroidPlayer;

            Assert.Equal(0, game.HandOf(me).Count);

            Assert.Throws<UnavailableActionException>( () => {
                CardPlacement.CreateRandomCommand(game, me);
            });
        }
    }
}
