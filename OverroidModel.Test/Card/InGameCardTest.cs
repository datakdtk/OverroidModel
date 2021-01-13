using OverroidModel.Card;
using OverroidModel.Card.Master;
using Xunit;

namespace OverroidModel.Test.Card
{
    public class InGameCardTest
    {
        [Fact]
        public void Test_Name()
        {
            var master = new Innocence();
            var card = new InGameCard(master);
            Assert.Equal(master.Name, card.Name);
        }

        [Fact]
        public void Test_Value_NotOverridden()
        {
            var master = new Innocence();
            var card = new InGameCard(master);
            Assert.Equal(master.Value, card.Value);
            Assert.Equal(master.Value, card.DefaultValue);
        }

        [Fact]
        public void Test_Value_Overridden()
        {
            var master = new Innocence();
            var card = new InGameCard(master);
            card.OverrideValue(0);
            Assert.NotEqual(master.Value, card.Value);
            Assert.Equal(0, card.Value);
            Assert.Equal(master.Value, card.DefaultValue);
        }

        [Fact]
        public void Test_Effect_NotOverridden()
        {
            var master = new Innocence();
            var card = new InGameCard(master);
            Assert.Equal(master.Effect, card.Effect);
            Assert.Equal(master.Effect, card.DefaultEffect);
        }

        [Fact]
        public void Test_Efect_Overridden()
        {
            var master = new Innocence();
            var card = new InGameCard(master);
            var anotherMaster = new Overroid();
            card.OverrideEffect(anotherMaster.Effect);
            Assert.NotEqual(master.Effect, card.Effect);
            Assert.Equal(anotherMaster.Effect, card.Effect);
            Assert.Equal(master.Effect, card.DefaultEffect);
        }

        [Fact]
        public void Test_SetGuessed()
        {
            var master = new Innocence();
            var card = new InGameCard(master);
            Assert.Equal(CardVisibility.Hidden, card.Visibility);
            card.SetGuessed();
            Assert.Equal(CardVisibility.Guessed, card.Visibility);
        }

        [Fact]
        public void Test_Reveal()
        {
            var master = new Innocence();
            var card = new InGameCard(master);
            Assert.Equal(CardVisibility.Hidden, card.Visibility);
            card.Reveal();
            Assert.Equal(CardVisibility.Revealed, card.Visibility);
        }

        [Fact]
        public void Test_SetGuessedAndReveal()
        {
            var master = new Innocence();
            var card = new InGameCard(master);
            card.SetGuessed();
            card.Reveal();
            Assert.Equal(CardVisibility.Revealed, card.Visibility);
        }

        [Fact]
        public void Test_RevealAndSetGuessed()
        {
            var master = new Innocence();
            var card = new InGameCard(master);
            card.Reveal();
            card.SetGuessed(); // still to be revealed
            Assert.Equal(CardVisibility.Revealed, card.Visibility);
        }

        [Fact]
        public void Test_ReturnDefault()
        {
            var master = new Innocence();
            var card = new InGameCard(master);
            var anotherMaster = new Overroid();

            card.OverrideValue(anotherMaster.Value);
            card.OverrideEffect(anotherMaster.Effect);
            
            Assert.NotEqual(master.Value, card.Value);
            Assert.NotEqual(master.Effect, card.Effect);

            card.ReturnToDefault();
            
            Assert.Equal(master.Value, card.Value);
            Assert.Equal(master.Effect, card.Effect);
        }
    }
}
