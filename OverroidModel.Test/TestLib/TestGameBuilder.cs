﻿using System;
using System.Collections.Generic;
using System.Linq;
using OverroidModel.Card;
using OverroidModel.GameAction;
using OverroidModel.GameAction.Commands;

namespace OverroidModel.Test.TestLib
{
    public static class TestGameBuilder
    {
        public static IndividualGame CreateIndividualGame(
            int round = 1,
            IEnumerable<CardName>? cardNamesInHumanHand = null,
            IEnumerable<CardName>? cardNamesInOverroidHand = null,
            bool detectionAvailable = true)
        {
            var humanPlayer = new PlayerAccount("human");
            var overroidPlayer = new PlayerAccount("overroid");

            var handSize = IndividualGame.maxRound + 1 - round;
            var humanHand = createPlayerHand(humanPlayer, cardNamesInHumanHand, handSize);
            var overroidHand = createPlayerHand(overroidPlayer, cardNamesInOverroidHand, handSize);

            var config = new TestConfig();
            config.DetectionAvailable = detectionAvailable;

            IMutableGame ig = new IndividualGame(
                humanPlayer: humanPlayer,
                overroidPlayer: overroidPlayer,
                humanPlayerHand: humanHand,
                overroidPlayerHand: overroidHand,
                hiddenCardMaster: new DummyCard(99),
                triggerCardMaster: null,
                config: config);

            while (ig.Battles.Count < round - 1)
            {
                ig.AddNewRound();
            }

            var g = (IndividualGame)ig;
            g.PushToActionStack(new RoundStart());
            g.ResolveStacks();
            return g;
        }

        public static PlayerHand createPlayerHand(PlayerAccount player, IEnumerable<CardName>? cardNamesInHand, int handSize)
        {
            var cards = cardNamesInHand != null
                ? cardNamesInHand.Select(n => CardDictionary.GetInGameCard(n, player)).ToList()
                : new List<InGameCard>();
            while (cards.Count < handSize)
            {
                cards.Add(new InGameCard(new DummyCard(99), player));
            }

            return new PlayerHand(player, cards);
        }

        public static void SetCardsToCurrentBattle(CardName attakingCardName, CardName defendingCardName, IMutableGame game)
        {
            var battle = game.CurrentBattle;
            game.ReceiveCommand(new CardPlacement(battle.AttackingPlayer, attakingCardName, null));
            game.ReceiveCommand(new CardPlacement(battle.DefendingPlayer, defendingCardName, null));
        }

    }
}
