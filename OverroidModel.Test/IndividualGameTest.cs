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
            game.AddCommandAuthorizer(new CommandAuthorizerImplement<DummyCommand>(player));
            Assert.NotNull(game.CommandRequirement);

            var command = new DummyCommand(player);
            game.ReceiveCommand(command);

            Assert.Null(game.CommandRequirement);
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

            var command = new CardPlacement(player, CardName.Diva);
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
            var command = new CardPlacement(player, CardName.Diva);
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
            var command1 = new CardPlacement(player1, CardName.Diva);
            game.ReceiveCommand(command1);

            var player2 = game.HumanPlayer;
            var command2 = new CardPlacement(player2, CardName.Idol);
            game.ReceiveCommand(command2);

            Assert.Equal(5, game.HandOf(player2).Count);
            Assert.False(game.HandOf(player2).HasCard(CardName.Idol));
            var battle = game.Battles[0];
            Assert.True(battle.HasCardOf(player2));
            Assert.Equal(CardName.Idol, battle.CardOf(player2).Name);
        }

        [Fact]
        public void Test_WaitingForDetectionAtRound1WhenDetectionIsAvailable()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva },
                cardNamesInHumanHand: new List<CardName>() { CardName.Idol },
                detectionAvailable: true
            );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Diva, CardName.Idol, game);
            CustomAssertion.WaitingForCommand<Detection>(game.HumanPlayer, game);
        }
 
        [Fact]
        public void Test_NotWaitingForDetectionAtRound1WhenDetectionIsUnavailable()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva },
                cardNamesInHumanHand: new List<CardName>() { CardName.Idol },
                detectionAvailable: false
            );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Diva, CardName.Idol, game);
            CustomAssertion.NotWaitingForCommand<Detection>(game);
        }

        [Fact]
        public void Test_WaitingForDetectionAtRound4WhenDetectionIsAvailable()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva },
                cardNamesInHumanHand: new List<CardName>() { CardName.Idol },
                detectionAvailable: true
            );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Idol, CardName.Diva, game);
            CustomAssertion.WaitingForCommand<Detection>(game.OverroidPlayer, game);
        }

        [Fact]
        public void Test_NotWaitingForDetectionAtRound5WhenDetectionIsAvailable()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 5,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva },
                cardNamesInHumanHand: new List<CardName>() { CardName.Idol },
                detectionAvailable: true
            );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Diva, CardName.Idol, game);
            CustomAssertion.NotWaitingForCommand<Detection>(game);
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
            var command1 = new CardPlacement(player1, CardName.Diva);
            game.ReceiveCommand(command1);

            var player2 = game.HumanPlayer;
            var command2 = new CardPlacement(player2, CardName.Idol);
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
                cardNamesInHumanHand: new List<CardName>() { CardName.Diva },
                detectionAvailable: true
            );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Idol, CardName.Diva, game);
            var detection = new Detection(game.HumanPlayer, CardName.Idol);
            game.ReceiveCommand(detection);
            game.ResolveAllActions(); // decide winner;

            CustomAssertion.WaitingForCommand<CardPlacement>(game.HumanPlayer, game); // next round has begun
            Assert.Equal(game.HumanPlayer, game.Battles[0].Winner);
            Assert.Equal(0, game.WinningStarOf(game.OverroidPlayer));
            Assert.Equal(2, game.WinningStarOf(game.HumanPlayer));

        }

        [Fact]
        public void Test_CanNullDetection()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol },
                cardNamesInHumanHand: new List<CardName>() { CardName.Diva },
                detectionAvailable: true
            );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Idol, CardName.Diva, game);
            var detection = new Detection(game.HumanPlayer, null);
            game.ReceiveCommand(detection);
            game.ResolveAllActions(); // decide winner;

            CustomAssertion.WaitingForCommand<CardPlacement>(game.HumanPlayer, game); // next round has begun
            Assert.Equal(game.HumanPlayer, game.Battles[0].Winner);
            Assert.Equal(0, game.WinningStarOf(game.OverroidPlayer));
            Assert.Equal(1, game.WinningStarOf(game.HumanPlayer));
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
            var command1 = new CardPlacement(player1, CardName.Diva);
            game.ReceiveCommand(command1);

            var player2 = game.HumanPlayer;
            var command2 = new CardPlacement(player2, CardName.Idol);
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
            var command1 = new CardPlacement(player1, CardName.Diva);
            game.ReceiveCommand(command1);

            var player2 = game.HumanPlayer;
            var command2 = new CardPlacement(player2, CardName.Idol);
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
            var command1 = new CardPlacement(player1, CardName.Idol);
            game.ReceiveCommand(command1);

            var player2 = game.OverroidPlayer;
            var command2 = new CardPlacement(player2, CardName.Diva);
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
            var command1 = new CardPlacement(player1, CardName.Diva);
            game.ReceiveCommand(command1);

            var player2 = game.HumanPlayer;
            var command2 = new CardPlacement(player2, CardName.Idol);
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
            var command1 = new CardPlacement(player1, CardName.Diva);
            game.ReceiveCommand(command1);

            var player2 = game.HumanPlayer;
            var command2 = new CardPlacement(player2, CardName.Idol);
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
            var command1 = new CardPlacement(player1, CardName.Spy);
            game.ReceiveCommand(command1);

            var player2 = game.HumanPlayer;
            var command2 = new CardPlacement(player2, CardName.Legion);
            game.ReceiveCommand(command2);

            while (game.CommandRequirement == null)
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
            var command1 = new CardPlacement(player1, CardName.Idol);
            game.ReceiveCommand(command1);

            var player2 = game.OverroidPlayer;
            var command2 = new CardPlacement(player2, CardName.Diva);
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
            var command1 = new CardPlacement(player1, CardName.Doctor);
            game.ReceiveCommand(command1);

            var player2 = game.HumanPlayer;
            var command2 = new CardPlacement(player2, CardName.Legion);
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
        public void Test_ResolveAllActions_LastActionIsGameEndWhenGameFinished()
        {
            IMutableGame game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Doctor },
                cardNamesInHumanHand: new List<CardName>() { CardName.Legion }
            );
            game.SetSpecialWinner(game.OverroidPlayer);
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Doctor, CardName.Legion, game);
            Assert.True(game.HasFinished());

            var action = game.ActionHistory.Last();
            Assert.IsType<GameEnd>(action);
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

        [Fact]
        public void Test_HasFinished_FalseIfNotAllRoundsAreOver()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 5,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol}, // Attacking first
                cardNamesInHumanHand: new List<CardName>() { CardName.Diva} // Defending first
            );
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Idol, CardName.Diva, game);
            Assert.False(game.HasFinished());
        }

        [Fact]
        public void Test_HasFinished_TrueIfNotAllRoundsAreOverAndSpecialWinnerIsSet()
        {
            IMutableGame game = TestGameBuilder.CreateIndividualGame(
                round: 5,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol}, // Attacking first
                cardNamesInHumanHand: new List<CardName>() { CardName.Diva} // Defending first
            );
            game.SetSpecialWinner(game.OverroidPlayer);
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Idol, CardName.Diva, game);

            Assert.True(game.HasFinished());
        }

        [Fact]
        public void Test_HasFinished_TrueIfAllRoundsAreOver()
        {
            IMutableGame game = TestGameBuilder.CreateIndividualGame(
                round: 6
            );
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Unknown, CardName.Unknown, game);

            Assert.True(game.HasFinished());
        }

        [Fact]
        public void Test_CheckWinner_WhenNotAllRoundsAreOver()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 5,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol}, // Attacking 
                cardNamesInHumanHand: new List<CardName>() { CardName.Diva} // Defending 
            );
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Idol, CardName.Diva, game);
            Assert.Null(game.CheckWinner());
        }

        [Fact]
        public void Test_CheckWinner_WhenNotAllRoundsAreOverAndSpecialWinnerIsSet()
        {
            IMutableGame game = TestGameBuilder.CreateIndividualGame(
                round: 5,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol}, // Attacking 
                cardNamesInHumanHand: new List<CardName>() { CardName.Diva} // Defending 
            );
            game.SetSpecialWinner(game.OverroidPlayer);
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Idol, CardName.Diva, game);
            Assert.True(game.WinningStarOf(game.HumanPlayer) > game.WinningStarOf(game.OverroidPlayer));
            Assert.Equal(game.OverroidPlayer, game.CheckWinner());
        }

        [Fact]
        public void Test_CheckWinner_WhenAllRoundsAreOverAndSpecialWinnerIsSet()
        {
            IMutableGame game = TestGameBuilder.CreateIndividualGame(
                round: 6,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol}, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Diva} // Attacking 
            );
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Diva, CardName.Idol, game);
            game.SetSpecialWinner(game.OverroidPlayer);
            Assert.True(game.WinningStarOf(game.HumanPlayer) > game.WinningStarOf(game.OverroidPlayer));
            Assert.Equal(game.OverroidPlayer, game.CheckWinner());
        }

        [Fact]
        public void Test_CheckWinner_WhenAllRoundsAreOverAndIsNotDrawGame()
        {
            IMutableGame game = TestGameBuilder.CreateIndividualGame(
                round: 6,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol}, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Diva} // Attacking 
            );
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Diva, CardName.Idol, game);
            Assert.True(game.WinningStarOf(game.HumanPlayer) > game.WinningStarOf(game.OverroidPlayer));
            Assert.Equal(game.HumanPlayer, game.CheckWinner());
        }

        [Fact]
        public void Test_CheckWinner_WhenAllRoundsAreOverAndIsDrawGame()
        {
            IMutableGame game = TestGameBuilder.CreateIndividualGame(
                round: 6
            );
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Unknown, CardName.Unknown, game);
            Assert.Equal(game.WinningStarOf(game.HumanPlayer), game.WinningStarOf(game.OverroidPlayer));
            Assert.Null(game.CheckWinner());
        }

        [Fact]
        public void Test_IsDrawGame_FalseWhenNotAllRoundsAreOver()
        {
            IMutableGame game = TestGameBuilder.CreateIndividualGame(
                round: 5
            );
            Assert.Equal(game.WinningStarOf(game.HumanPlayer), game.WinningStarOf(game.OverroidPlayer));
            Assert.False(game.IsDrawGame());
        }

        [Fact]
        public void Test_IsDrawGame_FalseWhenAllRoundsAreOverAndIsNotDrawGame()
        {
            IMutableGame game = TestGameBuilder.CreateIndividualGame(
                round: 6,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Idol}, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Diva} // Attacking 
            );
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Diva, CardName.Idol, game);
            Assert.True(game.WinningStarOf(game.HumanPlayer) > game.WinningStarOf(game.OverroidPlayer));
            Assert.False(game.IsDrawGame());
        }

        [Fact]
        public void Test_IsDrawGame_TrueWhenAllRoundsAreOverAndIsDrawGame()
        {
            IMutableGame game = TestGameBuilder.CreateIndividualGame(
                round: 6
            );
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Unknown, CardName.Unknown, game);
            Assert.Equal(game.WinningStarOf(game.HumanPlayer), game.WinningStarOf(game.OverroidPlayer));
            Assert.True(game.IsDrawGame());
        }

        [Fact]
        public void Test_IsDrawGame_WhenlSpecialWinnerIsSet()
        {
            IMutableGame game = TestGameBuilder.CreateIndividualGame(
                round: 6
            );
            TestGameBuilder.SetCardsToCurrentBattle(CardName.Unknown, CardName.Unknown, game);
            game.SetSpecialWinner(game.OverroidPlayer);
            Assert.Equal(game.WinningStarOf(game.HumanPlayer), game.WinningStarOf(game.OverroidPlayer));
            Assert.False(game.IsDrawGame());
        }

        [Fact]
        public void Test_DetectionIsAvailableInRound_ReturnsTrueInRound1IfAvailable()
        {
            var game = TestGameBuilder.CreateIndividualGame(detectionAvailable: true);
            Assert.True(game.DetectionIsAvailableInRound(1));
        }
 
        [Fact]
        public void Test_DetectionIsAvailableInRound_ReturnsTrueInRound4IfAvailable()
        {
            var game = TestGameBuilder.CreateIndividualGame(detectionAvailable: true);
            Assert.True(game.DetectionIsAvailableInRound(4));
        }
 
        [Fact]
        public void Test_DetectionIsAvailableInRound_ReturnsFalseInRound5IfAvailable()
        {
            var game = TestGameBuilder.CreateIndividualGame(detectionAvailable: true);
            Assert.False(game.DetectionIsAvailableInRound(5));
        }
 
        [Fact]
        public void Test_DetectionIsAvailableInRound_ReturnsFalseInRound1IfNotAvailable()
        {
            var game = TestGameBuilder.CreateIndividualGame(detectionAvailable: false);
            Assert.False(game.DetectionIsAvailableInRound(1));
        }

        [Fact]
        public void Test_DisableRoundEffect_AvailableInFirstRound()
        {
            IMutableGame game = TestGameBuilder.CreateIndividualGame(round: 1);
            Assert.False(game.EffectIsDisabled(1, game.HumanPlayer));
            game.DisableRoundEffects(1, game.HumanPlayer);
            Assert.True(game.EffectIsDisabled(1, game.HumanPlayer));
        }

        [Fact]
        public void Test_DisableRoundEffect_AvailableInFutureRound()
        {
            IMutableGame game = TestGameBuilder.CreateIndividualGame(round: 1);
            Assert.False(game.EffectIsDisabled(2, game.HumanPlayer));
            game.DisableRoundEffects(2, game.HumanPlayer);
            Assert.False(game.EffectIsDisabled(1, game.HumanPlayer));
            Assert.True(game.EffectIsDisabled(2, game.HumanPlayer));
        }
 
        [Fact]
        public void Test_DisableRoundEffect_OtherPlayerIsNotAffected()
        {
            IMutableGame game = TestGameBuilder.CreateIndividualGame(round: 1);
            Assert.False(game.EffectIsDisabled(1, game.HumanPlayer));
            Assert.False(game.EffectIsDisabled(1, game.OverroidPlayer));
            game.DisableRoundEffects(1, game.HumanPlayer);
            Assert.True(game.EffectIsDisabled(1, game.HumanPlayer));
            Assert.False(game.EffectIsDisabled(1, game.OverroidPlayer));
        }

        [Fact]
        public void Test_DisableRoundEffect_AvailableInLastRound()
        {
            IMutableGame game = TestGameBuilder.CreateIndividualGame(round: 6);
            Assert.False(game.EffectIsDisabled(6, game.HumanPlayer));
            game.DisableRoundEffects(6, game.HumanPlayer);
            Assert.True(game.EffectIsDisabled(6, game.HumanPlayer));
        }
   }
}
