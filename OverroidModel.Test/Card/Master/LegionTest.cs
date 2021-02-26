using System;
using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.GameAction.Effects;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.Card.Master
{
    public class LegionTest
    {
        [Fact]
        public void Test_EffectTriggersWhenWon()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Legion }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Attacking
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Innocence, CardName.Legion, game);

            CustomAssertion.WinsInLastRound(CardName.Legion, game);
            CustomAssertion.ActionIsInHistory<TrampleEffect>(game.ActionHistory);
            Assert.True(game.EffectIsDisabled(3, game.HumanPlayer));
        }

        [Fact]
        public void Test_EffectDoesNotTriggerWhenLost()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Legion } // Defending
                );

            if (game.HandOf(game.OverroidPlayer).CardOf(CardName.Unknown)?.Value! < 12)
            {
                throw new Exception("Value of dummy card is not strong enough");
            }

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Unknown, CardName.Legion, game);

            CustomAssertion.LosesInLastRound(CardName.Legion, game);
            CustomAssertion.ActionIsNotInHistory<TrampleEffect>(game.ActionHistory);
            Assert.False(game.EffectIsDisabled(3, game.HumanPlayer));
        }
 
        [Fact]
        public void Test_EffectDoesNotTriggerWhenWonAtLastRound()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: IndividualGame.MAX_ROUND,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Legion }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Attacking
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Innocence, CardName.Legion, game);

            CustomAssertion.WinsInLastRound(CardName.Legion, game);
            CustomAssertion.ActionIsNotInHistory<TrampleEffect>(game.ActionHistory);
        }
    }
}
