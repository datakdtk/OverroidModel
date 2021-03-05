using System;
using System.Collections.Generic;
using System.Linq;
using OverroidModel.Card;
using OverroidModel.GameAction;
using OverroidModel.GameAction.Effects;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.GameAction.Effects
{
    public class BattleWinEffectTest
    {
        [Fact]
        public void Test_BattleEndActionIsTakenWhenTriggeredInLastRound()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 6,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva },
                cardNamesInHumanHand: new List<CardName>() { CardName.Legion }
            );
            var battleEndCount = game.ActionHistory.OfType<BattleEnd>().Count();

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Legion, CardName.Diva, game);

            CustomAssertion.WinsInLastRound(CardName.Diva, game);
            CustomAssertion.ActionIsInHistory<InspirationEfect>(game.ActionHistory);
            var battleEndCount2 = game.ActionHistory.OfType<BattleEnd>().Count();
            Assert.Equal(battleEndCount + 1, battleEndCount2);
        }
    }
}
