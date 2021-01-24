using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.Game.Actions;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.Card.Master
{
    public class DeathTest
    {
        [Fact]
        public void Test_LosesToInnocenceWhenAttacking()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Death }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Death, CardName.Innocence, game);

            CustomAssertion.LosesInLastRound(CardName.Death, game);
            CustomAssertion.ActionIsInHistory<SnipeEffect>(game.ActionHistory);
            CustomAssertion.ActionIsNotInHistory<MiracleEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_WinsToInnocenceWhenDefending()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Death }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Innocence } // Attacking
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Innocence, CardName.Death, game);

            CustomAssertion.WinsInLastRound(CardName.Death, game);
            CustomAssertion.ActionIsNotInHistory<SnipeEffect>(game.ActionHistory);
            CustomAssertion.ActionIsNotInHistory<MiracleEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_WinsToTricksterWhenAttacking()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Death }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Trickster } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Death, CardName.Trickster, game);

            CustomAssertion.WinsInLastRound(CardName.Death, game);
            CustomAssertion.ActionIsInHistory<SnipeEffect>(game.ActionHistory);
            CustomAssertion.ActionIsInHistory<ReversalEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_LosesToTricksterWhenDefending()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Death }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Trickster } // Attacking
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Trickster, CardName.Death, game);

            CustomAssertion.LosesInLastRound(CardName.Death, game);
            CustomAssertion.ActionIsNotInHistory<SnipeEffect>(game.ActionHistory);
            CustomAssertion.ActionIsInHistory<ReversalEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_LosesToOverroidWhenAttacking()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 1,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Death }, // Attacking
                cardNamesInHumanHand: new List<CardName>() { CardName.Overroid } // Defending
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Death, CardName.Overroid, game);

            CustomAssertion.LosesInLastRound(CardName.Death, game);
            CustomAssertion.ActionIsInHistory<SnipeEffect>(game.ActionHistory);
            CustomAssertion.ActionIsInHistory<SingularityEffect>(game.ActionHistory);
        }

        [Fact]
        public void Test_WinsToOverroidWhenDefending()
        {
            var game = TestGameBuilder.CreateIndividualGame(
                round: 2,
                cardNamesInOverroidHand: new List<CardName>() { CardName.Death }, // Defending
                cardNamesInHumanHand: new List<CardName>() { CardName.Overroid } // Attacking
                );

            TestGameBuilder.SetCardsToCurrentBattle(CardName.Overroid, CardName.Death, game);

            CustomAssertion.WinsInLastRound(CardName.Death, game);
            CustomAssertion.ActionIsNotInHistory<SnipeEffect>(game.ActionHistory);
            CustomAssertion.ActionIsNotInHistory<SingularityEffect>(game.ActionHistory);
        }
    }
}
