using System;
using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.Game.Actions;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.Card.Master
{
    public class DivaTest
    {

        [Fact]
        public void Test_WinsToInnocence()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Diva, CardName.Innocence, game);

            CustomAssertion.WinsInLastRound(CardName.Diva, game);
            CustomAssertion.ActionIsNotInHistory<InspirationEfect>(game.ActionHistory);
        }

        [Fact]
        public void Test_LosesToGreaterCard()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva } // Attacking
                );

            if (game.HandOf(game.OverroidPlayer).CardOf(CardName.Unknown)?.Value! < 12)
            {
                throw new Exception("Value of dummy card is not strong enough");
            }

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Diva, CardName.Unknown, game);


            CustomAssertion.LosesInLastRound(CardName.Diva, game);
            CustomAssertion.ActionIsNotInHistory<InspirationEfect>(game.ActionHistory);
        }

        [Fact]
        public void Test_WinsToLegionByEffect()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Legion } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Diva, CardName.Legion, game);

            CustomAssertion.WinsInLastRound(CardName.Diva, game);
            CustomAssertion.ActionIsInHistory<InspirationEfect>(game.ActionHistory);
        }

        [Fact]
        public void Test_NextRoundEffectIsNotDisabledAfterBattleWithLegion()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Diva }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Legion } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Diva, CardName.Legion, game);

            CustomAssertion.ActionIsNotInHistory<TrampleEffect>(game.ActionHistory);
            Assert.False(game.EffectIsDisabled(2, game.OverroidPlayer));
        }
    }
}
