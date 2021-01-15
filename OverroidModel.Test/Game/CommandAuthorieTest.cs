using OverroidModel.Card;
using OverroidModel.Exceptions;
using OverroidModel.Game;
using OverroidModel.Game.Actions.Commands;
using Xunit;

namespace OverroidModel.Test.Game
{
    public class CommandAuthorieTest
    {
        [Fact]
        public void Test_Authorize_Pass()
        {
            var p= new PlayerAccount("hoge");
            var a = new CommandAuthorizer<CardPlacement>(p);

            var c = new CardPlacement(p, CardName.Inocent, null);
            a.Authorize(c);

            Assert.True(true); // Expected not to be thrown;
        }

        [Fact]
        public void Test_Authorize_DifferntPlayer()
        {
            var p= new PlayerAccount("hoge");
            var a = new CommandAuthorizer<CardPlacement>(p);

            var anotherP = new PlayerAccount("fuga");
            var c = new CardPlacement(anotherP, CardName.Inocent, null);
            Assert.Throws<UnavailableActionException>(() => a.Authorize(c));
        }

        [Fact]
        public void Test_Authorize_DifferntCommandClass()
        {
            var p= new PlayerAccount("hoge");
            var a = new CommandAuthorizer<CardPlacement>(p);

            var c = new RushCommand(p, CardName.Inocent);
            Assert.Throws<UnavailableActionException>(() => a.Authorize(c));
        }
    }
}
