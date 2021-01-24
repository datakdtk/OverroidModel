using OverroidModel.Card;
using OverroidModel.Exceptions;
using OverroidModel.GameAction.Commands;
using Xunit;

namespace OverroidModel.Test
{
    public class CommandAuthorieTest
    {
        [Fact]
        public void Test_RequiredCommandInfo_SameType()
        {
            var p = new PlayerAccount("hoge");
            var a = new CommandAuthorizer<CardPlacement>(p);

            var result = a.RequiredCommandInfo;
            Assert.Equal(typeof(CardPlacement), result.Item1);
            Assert.Equal(p, result.Item2);
        }

        [Fact]
        public void Test_RequiredCommandInfo_OtherType()
        {
            var p = new PlayerAccount("hoge");
            var a = new CommandAuthorizer<HackCommand>(p);

            var result = a.RequiredCommandInfo;
            Assert.NotEqual(typeof(CardPlacement), result.Item1);
        }

        [Fact]
        public void Test_Authorize_Pass()
        {
            var p = new PlayerAccount("hoge");
            var a = new CommandAuthorizer<CardPlacement>(p);

            var c = new CardPlacement(p, CardName.Innocence, null);
            a.Authorize(c);

            Assert.True(true); // Expected not to be thrown;
        }

        [Fact]
        public void Test_Authorize_DifferntPlayer()
        {
            var p = new PlayerAccount("hoge");
            var a = new CommandAuthorizer<CardPlacement>(p);

            var anotherP = new PlayerAccount("fuga");
            var c = new CardPlacement(anotherP, CardName.Innocence, null);
            Assert.Throws<UnavailableActionException>(() => a.Authorize(c));
        }

        [Fact]
        public void Test_Authorize_DifferntCommandClass()
        {
            var p = new PlayerAccount("hoge");
            var a = new CommandAuthorizer<CardPlacement>(p);

            var c = new RushCommand(p, CardName.Innocence);
            Assert.Throws<UnavailableActionException>(() => a.Authorize(c));
        }
    }
}
