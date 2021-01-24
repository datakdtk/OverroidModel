using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.GameAction;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.Card.Master
{
    public class TricksterTest
    {

        [Fact]
        public void Test_LosesToInnocence()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Trickster }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Trickster, CardName.Innocence, game);

            CustomAssertion.LosesInLastRound(CardName.Trickster, game);
            CustomAssertion.ActionIsInHistory<ReversalEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_WinsToDiva()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Trickster }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Diva } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Trickster, CardName.Diva, game);

            CustomAssertion.WinsInLastRound(CardName.Trickster, game);
            CustomAssertion.ActionIsInHistory<ReversalEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_WinsToOverroid()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Trickster }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Overroid } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Trickster, CardName.Overroid, game);

            CustomAssertion.WinsInLastRound(CardName.Trickster, game);
            CustomAssertion.ActionIsInHistory<ReversalEffect>(game.ActionHistory);
            CustomAssertion.ActionIsNotInHistory<SingularityEffect>(game.ActionHistory);
        }
    }
}
