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

            Assert.False(card.IsViewableTo(p1));
            Assert.False(card.IsViewableTo(p2));

            card.RevealTo(p1);
            
            Assert.True(card.IsViewableTo(p1));
            Assert.False(card.IsViewableTo(p2));
        }

        [Fact]
        public void Test_IsViewable_ToTwoPlaers()
        {
            var card = new OutsideCard(new Idol());
            var p1 = new PlayerAccount("hoge");
            var p2 = new PlayerAccount("fuga");

            Assert.False(card.IsViewableTo(p1));
            Assert.False(card.IsViewableTo(p2));

            card.RevealTo(p1);
            card.RevealTo(p2);
            
            Assert.True(card.IsViewableTo(p1));
            Assert.True(card.IsViewableTo(p2));
        }

        [Fact]
        public void Test_LookAtBy_WhenViewable()
        {
            var card = new OutsideCard(new Idol());
            var p = new PlayerAccount("hoge");

            card.RevealTo(p);
            Assert.True(card.IsViewableTo(p));

            var master = card.LookedAtBy(p);
            Assert.Equal(CardName.Idol, master.Name);
        }

        [Fact]
        public void Test_LookAtBy_WhenNotViewable()
        {
            var card = new OutsideCard(new Idol());
            var p = new PlayerAccount("hoge");

            Assert.Throws<UnavailableActionException>( () => card.LookedAtBy(p) );
        }
    }
}
