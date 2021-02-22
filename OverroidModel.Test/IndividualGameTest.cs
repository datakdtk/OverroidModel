using System;
using System.Collections.Generic;
using System.Linq;
using OverroidModel.Card;
using OverroidModel.GameAction;
using OverroidModel.GameAction.Commands;
using OverroidModel.GameAction.Effects;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test
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
            game.ResolveAllActions();

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
            game.ResolveAllActions(); // decide winner;

            Assert.Equal(player2, game.Battles[0].Winner);
            Assert.Equal(0, game.WinningStarOf(player1));
            Assert.Equal(2, game.WinningStarOf(player2));

        }

        [Fact]
        public void Test_NewRoundHasBegunAfterSecondPlayerPlacesCardAndResolveStacks()
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
            game.ResolveAllActions(); // decide winner;

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
            game.ResolveAllActions(); // decide winner;

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
            game.ResolveAllActions(); // decide winner;

            Assert.Equal(6, game.Battles.Count);
            Assert.Equal(6, game.CurrentBattle.Round);
            Assert.True(game.HasFinished());
        }

        [Fact]
        public void Test_ResolveNextAction_GameProceedByJustOneAction()
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

            var res1 = game.ResolveNextAction();
            Assert.IsType<CardOpen>(res1);
            Assert.Equal(player1, game.ActionHistory[game.ActionHistory.Count-1].Controller);

            var res2 = game.ResolveNextAction();
            Assert.IsType<CardOpen>(res2);
            Assert.Equal(player2, game.ActionHistory[game.ActionHistory.Count-1].Controller);
        }

        [Fact]
        public void Test_ResolveNextAction_ResolvedActionIsAddedToHistory()
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

            CustomAssertion.ActionIsNotInHistory<CardOpen>(game.ActionHistory);
            var res1 = game.ResolveNextAction();
            CustomAssertion.ActionIsInHistory<CardOpen>(game.ActionHistory);

            var lastHistory = game.ActionHistory[game.ActionHistory.Count - 1];

            Assert.IsType<CardOpen>(lastHistory);
            Assert.Equal(res1!.Controller, lastHistory.Controller);
            Assert.Equal(res1!.TargetCardName, lastHistory.TargetCardName);
        }

        [Fact]
        public void Test_ResolveNextAction_NotProceedAndReturnsFalseWhenWaitingForCommand()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Spy },
                cardNamesInHumanHand: new List<CardName>() { CardName.Legion }
            );

            var player1 = game.OverroidPlayer;
            var command1 = new CardPlacement(player1, CardName.Spy, null);
            game.ReceiveCommand(command1);

            var player2 = game.HumanPlayer;
            var command2 = new CardPlacement(player2, CardName.Legion, null);
            game.ReceiveCommand(command2);

            while (game.ExpectedCommandInfo == null)
            {
                var res = game.ResolveNextAction();
                if (res == null)
                {
                    throw new Exception("Failed to resolve action");
                }
            }

            var beforeHistoryCount = game.ActionHistory.Count;
            var result = game.ResolveNextAction();
            Assert.Null(result);
            var afterHistoryCount = game.ActionHistory.Count;
            Assert.Equal(beforeHistoryCount, afterHistoryCount);
        }

        [Fact]
        public void Test_ResolveNextAction_ReturnsFalseAfterGameEnd()
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
            game.ResolveAllActions(); // decide winner;

            Assert.True(game.HasFinished());
            Assert.Null(game.ResolveNextAction());
        }

        [Fact]
        public void Test_ResolveNextAction_IgnoresDisabledCardEffect()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Doctor },
                cardNamesInHumanHand: new List<CardName>() { CardName.Legion }
            );

            var player1 = game.OverroidPlayer;
            var command1 = new CardPlacement(player1, CardName.Doctor, null);
            game.ReceiveCommand(command1);

            var player2 = game.HumanPlayer;
            var command2 = new CardPlacement(player2, CardName.Legion, null);
            game.ReceiveCommand(command2);

            IGameAction? lastAction = null;
            while (lastAction is JammingEffect)
            {
                lastAction = game.ResolveNextAction();
                if (lastAction == null)
                {
                    throw new Exception("Failed to resolve action");
                }
            }

            var nextAction = game.ResolveNextAction();
            Assert.NotNull(nextAction);
            Assert.IsNotType<TrampleEffect>(nextAction);
        }

        [Fact]
        public void Test_AllCards_KeepTrackingMovedCard()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Overroid }
            );

            var player = game.OverroidPlayer;
            var command = new CardPlacement(player, CardName.Overroid, null);
            game.ReceiveCommand(command); // card moved to battle area from hand

            Assert.Equal(12, game.AllInGameCards.Count);
            Assert.Contains(game.AllInGameCards, c => c.Name == CardName.Overroid);
        }

        [Fact]
        public void Test_AllCards_RefrectsCardStateChanges()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Overroid }
            );

            var handCard = game.HandOf(game.OverroidPlayer).CardOf(CardName.Overroid)!;
            handCard.SetGuessed();

            var cardInAllCardList = game.AllInGameCards.First(c => c.Name == CardName.Overroid);
            Assert.True(cardInAllCardList.IsGuessable());
        }
    }
}
