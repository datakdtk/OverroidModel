using System;
using System.Collections.Generic;
using System.Linq;
using OverroidModel.Card;
using OverroidModel.Game;
using OverroidModel.Game.Actions;
using OverroidModel.Game.Actions.Commands;

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

            var handSize = IGameInformation.maxRound + 1 - round;
            var humanHand = createPlayerHand(cardNamesInHumanHand, handSize);
            var overroidHand = createPlayerHand(cardNamesInOverroidHand, handSize);

            var config = new TestConfig();
            config.DetectionAvailable = detectionAvailable;

            IMutableGame ig = new IndividualGame(
                humanPlayer: humanPlayer,
                overroidPlayer: overroidPlayer,
                humanPlayerHand: humanHand,
                overroidPlayerHand: overroidHand,
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

        public static PlayerHand createPlayerHand(IEnumerable<CardName>? cardNamesInHand, int handSize)
        {
            var cards = cardNamesInHand != null
                ? cardNamesInHand.Select(n => CardDictionary.GetInGameCard(n)).ToList()
                : new List<InGameCard>();
            while (cards.Count < handSize)
            {
                cards.Add(new InGameCard(new DummyCard(5)));
            }

            return new PlayerHand(cards);
        }

        public static void SetCardsToCurrentBattle(CardName attakingCardName, CardName defendingCardName, IMutableGame game)
        {
            var battle = game.CurrentBattle;
            game.ReceiveCommand(new CardPlacement(battle.AttackingPlayer, attakingCardName, null));
            game.ReceiveCommand(new CardPlacement(battle.DefendingPlayer, defendingCardName, null));
        }

    }
}
