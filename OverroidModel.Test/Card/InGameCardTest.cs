using OverroidModel.Card;
using OverroidModel.Card.Master;
using OverroidModel.Exceptions;
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
        public void Test_RevealByHack_FromHidden()
        {
            var master = new Innocence();
            var card = new InGameCard(master);
            Assert.Equal(CardVisibility.Hidden, card.Visibility);
            card.RevealByHack();
            Assert.Equal(CardVisibility.Hacked, card.Visibility);
        }

        [Fact]
        public void Test_RevealByHack_FromGuessed()
        {
            var master = new Innocence();
            var card = new InGameCard(master);
            card.SetGuessed();
            Assert.Equal(CardVisibility.Guessed, card.Visibility);
            card.RevealByHack();
            Assert.Equal(CardVisibility.Hacked, card.Visibility);
        }

        [Fact]
        public void Test_SetGuessed_FromRevealed()
        {
            var master = new Innocence();
            var card = new InGameCard(master);
            card.RevealByHack();
            Assert.Equal(CardVisibility.Hacked, card.Visibility);
            card.SetGuessed(); // still to be revealed
            Assert.Equal(CardVisibility.Hacked, card.Visibility);
        }

        [Fact]
        public void Test_SetGuessed_FromOpened()
        {
            var master = new Innocence();
            var card = new InGameCard(master);
            card.Open();
            Assert.Equal(CardVisibility.Opened, card.Visibility);
            card.SetGuessed(); // still to be revealed
            Assert.Equal(CardVisibility.Guessed, card.Visibility);
        }

        [Fact]
        public void Test_Open_FromHidden()
        {
            var master = new Innocence();
            var card = new InGameCard(master);
            Assert.Equal(CardVisibility.Hidden, card.Visibility);
            card.Open();
            Assert.Equal(CardVisibility.Opened, card.Visibility);
        }

        [Fact]
        public void Test_Open_FromGuessed()
        {
            var master = new Innocence();
            var card = new InGameCard(master);
            card.SetGuessed();
            Assert.Equal(CardVisibility.Guessed, card.Visibility);
            card.Open();
            Assert.Equal(CardVisibility.Opened, card.Visibility);
        }

        [Fact]
        public void Test_Open_FromHacked()
        {
            var master = new Innocence();
            var card = new InGameCard(master);
            card.RevealByHack();
            Assert.Equal(CardVisibility.Hacked, card.Visibility);
            card.Open();
            Assert.Equal(CardVisibility.Opened, card.Visibility);
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

        [Fact]
        public void Test_SetTrueDataIfUnkown_CanUpdateUnkownCard()
        {
            var master = new UnknownCard();
            var card = new InGameCard(master);

            card.SetTrueDataIfUnknown(CardName.Overroid);

            Assert.False(card.IsUnknown());
            Assert.Equal(CardName.Overroid, card.Name);
        }

        [Fact]
        public void Test_SetTrueDataIfUnkown_NothingHappensWhenGivenSameCardName()
        {
            var master = new Overroid();
            var card = new InGameCard(master);

            card.SetTrueDataIfUnknown(CardName.Overroid);

            Assert.False(card.IsUnknown());
            Assert.Equal(CardName.Overroid, card.Name);
        }

        [Fact]
        public void Test_SetTrueDataIfUnkown_ThrowsWhenTriedToKnownCard()
        {
            var master = new Innocence();
            var card = new InGameCard(master);

            Assert.Throws<GameLogicException>(() => card.SetTrueDataIfUnknown(CardName.Overroid));
        }

        [Fact]
        public void Test_IsUnknown_True()
        {
            var master = new UnknownCard();
            var card = new InGameCard(master);

            Assert.True(card.IsUnknown());
        }

        [Fact]
        public void Test_IsUnknown_False()
        {
            var master = new Innocence();
            var card = new InGameCard(master);

            Assert.False(card.IsUnknown());
        }
    }
}
