using OverroidModel.Card;
using OverroidModel.Exceptions;
using OverroidModel.GameAction.Commands;
using Xunit;

namespace OverroidModel.Test
{
    public class CommandAuthorizerTest
    {
        [Fact]
        public void Test_CommandRequirement()
        {
            var player = new PlayerAccount("hoge");
            var authorizer = new CommandAuthorizerImplement<CardPlacement>(player);

            var requirement = authorizer.CommandRequirement;
            Assert.IsType<CommandAuthorizerImplement<CardPlacement>>(authorizer);
            Assert.Equal(typeof(CardPlacement), requirement.CommandType);
            Assert.Equal(player, requirement.ComandingPlayer);
        }

        [Fact]
        public void Test_Authorize_Pass()
        {
            var player = new PlayerAccount("hoge");
            var authorizer = new CommandAuthorizerImplement<CardPlacement>(player);

            var command = new CardPlacement(player, CardName.Innocence);
            authorizer.Authorize(command);

            Assert.True(true); // Expected not to be thrown;
        }

        [Fact]
        public void Test_Authorize_DifferntPlayer()
        {
            var player = new PlayerAccount("hoge");
            var authorizer = new CommandAuthorizerImplement<CardPlacement>(player);

            var anotherPlayer = new PlayerAccount("fuga");
            var command = new CardPlacement(anotherPlayer, CardName.Innocence);
            Assert.Throws<UnavailableActionException>(() => authorizer.Authorize(command));
        }

        [Fact]
        public void Test_Authorize_DifferntCommandClass()
        {
            var player = new PlayerAccount("hoge");
            var authorizer = new CommandAuthorizerImplement<CardPlacement>(player);

            var command = new RushCommand(player, CardName.Innocence);
            Assert.Throws<UnavailableActionException>(() => authorizer.Authorize(command));
        }
    }
}
