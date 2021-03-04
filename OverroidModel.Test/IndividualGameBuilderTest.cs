using OverroidModel.Card;
using OverroidModel.Exceptions;
using OverroidModel.GameAction;
using OverroidModel.GameAction.Commands;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test
{
    public class IndividualGameBuilderTest
    {
        [Fact]
        public void Test_InitializeGame_FirstActionIsGameStart()
        {
            var builder = new IndividualGameBuilder(new DummyShuffler());

            var humanPlayer = new PlayerAccount("hoge");
            var overroidPlayer = new PlayerAccount("fuga");
            var config = new TestConfig();

            var game = builder.InitializeGame(
                humanPlayer: humanPlayer,
                overroidPlayer: overroidPlayer,
                shufflingSeed: 12345,
                config: config
            );

            var firstAction = game.ResolveNextAction();
            Assert.IsType<GameStart>(firstAction);
        }

        [Fact]
        public void Test_InitializeGame_FirstRoundNotIsCreatedBeforeResolvingActions()
        {
            var builder = new IndividualGameBuilder(new DummyShuffler());

            var humanPlayer = new PlayerAccount("hoge");
            var overroidPlayer = new PlayerAccount("fuga");
            var config = new TestConfig();

            var game = builder.InitializeGame(
                humanPlayer: humanPlayer,
                overroidPlayer: overroidPlayer,
                shufflingSeed: 12345,
                config: config
            );

            Assert.Equal(0, game.Battles.Count);
            Assert.Throws<GameLogicException>(() => game.CurrentBattle);
        }

        [Fact]
        public void Test_InitializeGame_FirstRoundIsCreatedAfterResolvingActions()
        {
            var builder = new IndividualGameBuilder(new DummyShuffler());

            var humanPlayer = new PlayerAccount("hoge");
            var overroidPlayer = new PlayerAccount("fuga");
            var config = new TestConfig();

            var game = builder.InitializeGame(
                humanPlayer: humanPlayer,
                overroidPlayer: overroidPlayer,
                shufflingSeed: 12345,
                config: config
            );
            game.ResolveAllActions();

            Assert.Equal(1, game.Battles.Count);
            Assert.Equal(1, game.CurrentBattle.Round);
        }

        [Fact]
        public void Test_InitializeGame_OverroidAttacksInFirstRoundAfterResolvingActions()
        {
            var builder = new IndividualGameBuilder(new DummyShuffler());

            var humanPlayer = new PlayerAccount("hoge");
            var overroidPlayer = new PlayerAccount("fuga");
            var config = new TestConfig();

            var game = builder.InitializeGame(
                humanPlayer: humanPlayer,
                overroidPlayer: overroidPlayer,
                shufflingSeed: 12345,
                config: config
            );
            game.ResolveAllActions();

            Assert.Equal(overroidPlayer, game.CurrentBattle.AttackingPlayer);
            Assert.Equal(humanPlayer, game.CurrentBattle.DefendingPlayer);
        }
        
        [Fact]
        public void Test_InitializeGame_NotWaitingForCommandBeforeResolvingActions()
        {
            var builder = new IndividualGameBuilder(new DummyShuffler());

            var humanPlayer = new PlayerAccount("hoge");
            var overroidPlayer = new PlayerAccount("fuga");
            var config = new TestConfig();

            var game = builder.InitializeGame(
                humanPlayer: humanPlayer,
                overroidPlayer: overroidPlayer,
                shufflingSeed: 12345,
                config: config
            );

            Assert.Null(game.CommandRequirement);
        }


        [Fact]
        public void Test_InitializeGame_CheckExpectedCommandAfterResolvingActions()
        {
            var builder = new IndividualGameBuilder(new DummyShuffler());

            var humanPlayer = new PlayerAccount("hoge");
            var overroidPlayer = new PlayerAccount("fuga");
            var config = new TestConfig();

            var game = builder.InitializeGame(
                humanPlayer: humanPlayer,
                overroidPlayer: overroidPlayer,
                shufflingSeed: 12345,
                config: config
            );
            game.ResolveAllActions();

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
                shufflingSeed: 12345,
                config: config
            );

            var hand = game.HandOf(humanPlayer);
            Assert.Equal(6, hand.Count);
            Assert.True(hand.HasCard(CardName.Innocence));

            var card = hand.CardOf(CardName.Innocence);
            CustomAssertion.CardIsJustGuessable(card!);
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
                shufflingSeed: 12345,
                config: config
            );

            var hand = game.HandOf(overroidPlayer);
            Assert.Equal(6, hand.Count);
            Assert.True(hand.HasCard(CardName.Overroid));

            var card = hand.CardOf(CardName.Overroid);
            CustomAssertion.CardIsJustGuessable(card!);
        }

        [Fact]
        public void Test_InitializeGame_CheckHiddenCardWhenWatcherIsNotUsed()
        {
            var builder = new IndividualGameBuilder(new DummyShuffler());

            var humanPlayer = new PlayerAccount("hoge");
            var overroidPlayer = new PlayerAccount("fuga");
            var config = new TestConfig();

            var game = builder.InitializeGame(
                humanPlayer: humanPlayer,
                overroidPlayer: overroidPlayer,
                shufflingSeed: 12345,
                config: config
            );

            var hiddenCard = game.HiddenCard;
            Assert.False(hiddenCard.IsVisibleTo(humanPlayer));
            Assert.False(hiddenCard.IsVisibleTo(overroidPlayer));
        }
 
        [Fact]
        public void Test_InitializeGame_CheckTriggerCardWhenWatcherIsNotUsed()
        {
            var builder = new IndividualGameBuilder(new DummyShuffler());

            var humanPlayer = new PlayerAccount("hoge");
            var overroidPlayer = new PlayerAccount("fuga");
            var config = new TestConfig();

            var game = builder.InitializeGame(
                humanPlayer: humanPlayer,
                overroidPlayer: overroidPlayer,
                shufflingSeed: 12345,
                config: config
            );

            var triggerCard = game.TriggerCard;
            Assert.Null(triggerCard);
        }

        [Fact]
        public void Test_InitializeGame_CheckHiddenCardWhenWatcherIsUsed()
        {
            var builder = new IndividualGameBuilder(new DummyShuffler());

            var humanPlayer = new PlayerAccount("hoge");
            var overroidPlayer = new PlayerAccount("fuga");
            var config = new TestConfig();
            config.UsesWatcher = true;

            var game = builder.InitializeGame(
                humanPlayer: humanPlayer,
                overroidPlayer: overroidPlayer,
                shufflingSeed: 12345,
                config: config
            );

            var hiddenCard = game.HiddenCard;
            Assert.False(hiddenCard.IsVisibleTo(humanPlayer));
            Assert.False(hiddenCard.IsVisibleTo(overroidPlayer));
        }
 
        [Fact]
        public void Test_InitializeGame_CheckTriggerCardWhenWatcherIsUsed()
        {
            var builder = new IndividualGameBuilder(new DummyShuffler());

            var humanPlayer = new PlayerAccount("hoge");
            var overroidPlayer = new PlayerAccount("fuga");
            var config = new TestConfig();
            config.UsesWatcher = true;

            var game = builder.InitializeGame(
                humanPlayer: humanPlayer,
                overroidPlayer: overroidPlayer,
                shufflingSeed: 12345,
                config: config
            );

            var triggerCard = game.TriggerCard;
            Assert.NotNull(triggerCard);
            Assert.True(triggerCard!.IsVisibleTo(humanPlayer));
            Assert.True(triggerCard!.IsVisibleTo(overroidPlayer));
        }
    }
}
