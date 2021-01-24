using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.Game;
using OverroidModel.Game.Actions.Commands;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.Game
{
    public class IndividualGameTest
    {
        [Fact]
        public void Test_HasFinished_InitialState()
        {
            var game = TestGameBuilder.CreateIndividualGame();
            Assert.False(game.HasFinished());
        }

        [Fact]
        public void Test_ReceiveCommand()
        {
            IMutableGame game = TestGameBuilder.CreateIndividualGame();
            var player = game.HumanPlayer;
            game.AddCommandAuthorizer(new CommandAuthorizer<DummyCommand>(player));
            Assert.NotNull(game.ExpectedCommandInfo);

            var command = new DummyCommand(player);
            game.ReceiveCommand(command);

            Assert.Null(game.ExpectedCommandInfo);
            CustomAssertion.ActionIsInHistory<DummyCommand>(game.ActionHistory);
        }

        [Fact]
        public void Test_AttackingPlayerCanPlaceCard()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva }
                );
            var player = game.OverroidPlayer;

            var command = new CardPlacement(player, CardName.Diva, null);
            game.ReceiveCommand(command);

            Assert.Equal(5, game.HandOf(player).Count);
            Assert.False(game.HandOf(player).HasCard(CardName.Diva));
            Assert.True(game.CurrentBattle.HasCardOf(player));
            Assert.Equal(CardName.Diva, game.CurrentBattle.CardOf(player).Name);
        }

        [Fact]
        public void Test_WaitForDefendingPlayerToPlaceCardAfterAttackingPlayer()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva }
                );

            var player = game.OverroidPlayer;
            var command = new CardPlacement(player, CardName.Diva, null);
            game.ReceiveCommand(command);

            CustomAssertion.WaitingForCommand<CardPlacement>(game.OpponentOf(player), game);
        }

        [Fact]
        public void Test_DefendingPlayerCanPlaceCard()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva },
                cardNamesInHumanHand: new List<CardName>() { CardName.Idol }
                );

            var player1 = game.OverroidPlayer;
            var command1 = new CardPlacement(player1, CardName.Diva, null);
            game.ReceiveCommand(command1);

            var player2 = game.HumanPlayer;
            var command2 = new CardPlacement(player2, CardName.Idol, null);
            game.ReceiveCommand(command2);

            Assert.Equal(5, game.HandOf(player2).Count);
            Assert.False(game.HandOf(player2).HasCard(CardName.Idol));
            var battle = game.Battles[0];
            Assert.True(battle.HasCardOf(player2));
            Assert.Equal(CardName.Idol, battle.CardOf(player2).Name);
        }

        [Fact]
        public void Test_GraterPlayerWins()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva },
                cardNamesInHumanHand: new List<CardName>() { CardName.Idol }
                );

            var player1 = game.OverroidPlayer;
            var command1 = new CardPlacement(player1, CardName.Diva, null);
            game.ReceiveCommand(command1);

            var player2 = game.HumanPlayer;
            var command2 = new CardPlacement(player2, CardName.Idol, null);
            game.ReceiveCommand(command2);

            Assert.Equal(player1, game.Battles[0].Winner);
            Assert.Equal(1, game.WinningStarOf(player1));
            Assert.Equal(0, game.WinningStarOf(player2));
        }

        [Fact]
        public void Test_CanDetect()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol },
                cardNamesInHumanHand: new List<CardName>() { CardName.Diva }
                );

            var player1 = game.OverroidPlayer;
            var command1 = new CardPlacement(player1, CardName.Idol, null);
            game.ReceiveCommand(command1);

            var player2 = game.HumanPlayer;
            var command2 = new CardPlacement(player2, CardName.Diva, CardName.Idol);
            game.ReceiveCommand(command2);

            Assert.Equal(player2, game.Battles[0].Winner);
            Assert.Equal(0, game.WinningStarOf(player1));
            Assert.Equal(2, game.WinningStarOf(player2));

        }

        [Fact]
        public void Test_NewRoundHasBegunAfterSecondPlayerPlacesCard()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva },
                cardNamesInHumanHand: new List<CardName>() { CardName.Idol }
                );

            var player1 = game.OverroidPlayer;
            var command1 = new CardPlacement(player1, CardName.Diva, null);
            game.ReceiveCommand(command1);

            var player2 = game.HumanPlayer;
            var command2 = new CardPlacement(player2, CardName.Idol, null);
            game.ReceiveCommand(command2);

            Assert.Equal(2, game.Battles.Count);
            Assert.Equal(2, game.CurrentBattle.Round);
            Assert.False(game.HasFinished());
        }

        [Fact]
        public void Test_PlayerPlaceCardAfterSecondPlayerPlacesCard()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva },
                cardNamesInHumanHand: new List<CardName>() { CardName.Idol }
                );

            var player1 = game.OverroidPlayer;
            var command1 = new CardPlacement(player1, CardName.Diva, null);
            game.ReceiveCommand(command1);

            var player2 = game.HumanPlayer;
            var command2 = new CardPlacement(player2, CardName.Idol, null);
            game.ReceiveCommand(command2);

            CustomAssertion.WaitingForCommand<CardPlacement>(player2, game);
        }

        [Fact]
        public void Test_GameFinishesAfterLastRoundEnds()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 6,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva },
                cardNamesInHumanHand: new List<CardName>() { CardName.Idol }
                );

            var player1 = game.HumanPlayer;
            var command1 = new CardPlacement(player1, CardName.Idol, null);
            game.ReceiveCommand(command1);

            var player2 = game.OverroidPlayer;
            var command2 = new CardPlacement(player2, CardName.Diva, null);
            game.ReceiveCommand(command2);

            Assert.Equal(6, game.Battles.Count);
            Assert.Equal(6, game.CurrentBattle.Round);
            Assert.True(game.HasFinished());
        }
    }
}
