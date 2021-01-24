using OverroidModel.Card;
using OverroidModel.Game;
using OverroidModel.Game.Actions.Commands;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.Game
{
    public class IndividualGameBuilderTest
    {
        [Fact]
        public void Test_InitializeGame_FirstRoundIsCreated()
        {
            var builder = new IndividualGameBuilder(new DummyShuffler());

            var humanPlayer = new PlayerAccount("hoge");
            var overroidPlayer = new PlayerAccount("fuga");
            var config = new TestConfig();

            var game = builder.InitializeGame(
                humanPlayer: humanPlayer,
                overroidPlayer: overroidPlayer,
                shufflingSeed: "dummySeed",
                config: config
                );

            Assert.Equal(1, game.Battles.Count);
            Assert.Equal(1, game.CurrentBattle.Round);
        }

        [Fact]
        public void Test_InitializeGame_OverroidAttacksInFirstRound()
        {
            var builder = new IndividualGameBuilder(new DummyShuffler());

            var humanPlayer = new PlayerAccount("hoge");
            var overroidPlayer = new PlayerAccount("fuga");
            var config = new TestConfig();

            var game = builder.InitializeGame(
                humanPlayer: humanPlayer,
                overroidPlayer: overroidPlayer,
                shufflingSeed: "dummySeed",
                config: config
                );

            Assert.Equal(overroidPlayer, game.CurrentBattle.AttackingPlayer);
            Assert.Equal(humanPlayer, game.CurrentBattle.DefendingPlayer);
        }

        [Fact]
        public void Test_InitializeGame_CheckExpectedCommand()
        {
            var builder = new IndividualGameBuilder(new DummyShuffler());

            var humanPlayer = new PlayerAccount("hoge");
            var overroidPlayer = new PlayerAccount("fuga");
            var config = new TestConfig();

            var game = builder.InitializeGame(
                humanPlayer: humanPlayer,
                overroidPlayer: overroidPlayer,
                shufflingSeed: "dummySeed",
                config: config
                );

            CustomAssertion.WaitingForCommand<CardPlacement>(overroidPlayer, game);
        }

        [Fact]
        public void Test_InitializeGame_CheckHumanPlayersHand()
        {
            var builder = new IndividualGameBuilder(new DummyShuffler());

            var humanPlayer = new PlayerAccount("hoge");
            var overroidPlayer = new PlayerAccount("fuga");
            var config = new TestConfig();

            var game = builder.InitializeGame(
                humanPlayer: humanPlayer,
                overroidPlayer: overroidPlayer,
                shufflingSeed: "dummySeed",
                config: config
                );

            var hand= game.HandOf(humanPlayer);
            Assert.Equal(6, hand.Count);
            Assert.True(hand.HasCard(CardName.Innocence));

            var card = hand.CardOf(CardName.Innocence);
            Assert.Equal(CardVisibility.Guessed, card?.Visibility);
        }

        [Fact]
        public void Test_InitializeGame_CheckOverroidPlayersHand()
        {
            var builder = new IndividualGameBuilder(new DummyShuffler());

            var humanPlayer = new PlayerAccount("hoge");
            var overroidPlayer = new PlayerAccount("fuga");
            var config = new TestConfig();

            var game = builder.InitializeGame(
                humanPlayer: humanPlayer,
                overroidPlayer: overroidPlayer,
                shufflingSeed: "dummySeed",
                config: config
                );

            var hand= game.HandOf(overroidPlayer);
            Assert.Equal(6, hand.Count);
            Assert.True(hand.HasCard(CardName.Overroid));

            var card = hand.CardOf(CardName.Overroid);
            Assert.Equal(CardVisibility.Guessed, card?.Visibility);
        }
    }
}
