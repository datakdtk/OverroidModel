using OverroidModel.Card;
using OverroidModel.Card.Master;
using OverroidModel.Exceptions;
using Xunit;

namespace OverroidModel.Test.Card
{
    public class OutsideCardTest
    {
        [Fact]
        public void Test_IsViewable_ToOnePlayer()
        {
            var card = new OutsideCard(new Idol());
            var p1 = new PlayerAccount("hoge");
            var p2 = new PlayerAccount("fuga");

            Assert.False(card.IsVisibleTo(p1));
            Assert.False(card.IsVisibleTo(p2));

            card.RevealTo(p1);
            
            Assert.True(card.IsVisibleTo(p1));
            Assert.False(card.IsVisibleTo(p2));
        }

        [Fact]
        public void Test_IsViewable_ToTwoPlaers()
        {
            var card = new OutsideCard(new Idol());
            var p1 = new PlayerAccount("hoge");
            var p2 = new PlayerAccount("fuga");

            Assert.False(card.IsVisibleTo(p1));
            Assert.False(card.IsVisibleTo(p2));

            card.RevealTo(p1);
            card.RevealTo(p2);
            
            Assert.True(card.IsVisibleTo(p1));
            Assert.True(card.IsVisibleTo(p2));
        }

        [Fact]
        public void Test_Open()
        {
            var card = new OutsideCard(new Idol());
            Assert.False(card.IsOpened());
            card.Open();
            Assert.True(card.IsOpened());
        }

        [Fact]
        public void Test_IsViewable_WhenOpened()
        {
            var card = new OutsideCard(new Idol());
            var p1 = new PlayerAccount("hoge");

            Assert.False(card.IsVisibleTo(p1));

            card.Open();
            
            Assert.True(card.IsVisibleTo(p1));
        }
    }
}
