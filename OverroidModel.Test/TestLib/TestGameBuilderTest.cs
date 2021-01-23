using System.Collections.Generic;
using OverroidModel.Card;
using OverroidModel.Game.Actions.Commands;
using Xunit;

namespace OverroidModel.Test.TestLib
{
    public class TestGameBuilderTest
    {

        [Fact]
        public void TestCreateIndividualGame_CanCreateFirstRound()
        {
            var game = TestGameBuilder.CreateIndividualGame();

            Assert.Equal(1, game.Battles.Count);
            Assert.Equal(1, game.CurrentBattle.Round);
            Assert.Equal(typeof(CardPlacement), game.ExpectedCommandInfo?.type);
            Assert.Equal(6, game.HandOf(game.HumanPlayer).Count);
            Assert.Equal(6, game.HandOf(game.OverroidPlayer).Count);
        }

        [Fact]
        public void TestCreateIndividualGame_CanCreateSecondRound()
        {
            var game = TestGameBuilder.CreateIndividualGame(round: 2);

            Assert.Equal(2, game.Battles.Count);
            Assert.Equal(2, game.CurrentBattle.Round);
            Assert.Equal(typeof(CardPlacement), game.ExpectedCommandInfo?.type);
            Assert.Equal(5, game.HandOf(game.HumanPlayer).Count);
            Assert.Equal(5, game.HandOf(game.OverroidPlayer).Count);
        }

        [Fact]
        public void TestCreateIndividualGame_CanSpecifyCardInHumanHand()
        {
            var cardNames = new List<CardName>() { CardName.Inocence, CardName.Hacker };
            var game = TestGameBuilder.CreateIndividualGame(cardNamesInHumanHand: cardNames);

            var hand = game.HandOf(game.HumanPlayer);
            Assert.True(hand.HasCard(cardNames[0]));
            Assert.True(hand.HasCard(cardNames[1]));
        }

        [Fact]
        public void TestCreateIndividualGame_CanSpecifyCardInOverroidHand()
        {
            var cardNames = new List<CardName>() { CardName.Overroid, CardName.Death };
            var game = TestGameBuilder.CreateIndividualGame(cardNamesInOverroidHand: cardNames);

            var hand = game.HandOf(game.OverroidPlayer);
            Assert.True(hand.HasCard(cardNames[0]));
            Assert.True(hand.HasCard(cardNames[1]));
        }
    }
}
