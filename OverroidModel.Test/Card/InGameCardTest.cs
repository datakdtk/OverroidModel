﻿using OverroidModel.Card;
using OverroidModel.Card.Master;
using OverroidModel.Test.TestLib;
using Xunit;

namespace OverroidModel.Test.Card
{
    public class InGameCardTest
    {
        [Fact]
        public void Test_Name()
        {
            var master = new Innocence();
            var card = new InGameCard(master, new PlayerAccount("hoge"));
            Assert.Equal(master.Name, card.Name);
        }

        [Fact]
        public void Test_Value_NotOverridden()
        {
            var master = new Innocence();
            var card = new InGameCard(master, new PlayerAccount("hoge"));
            Assert.Equal(master.Value, card.Value);
            Assert.Equal(master.Value, card.DefaultValue);
        }

        [Fact]
        public void Test_Value_Overridden()
        {
            var master = new Innocence();
            var card = new InGameCard(master, new PlayerAccount("hoge"));
            card.OverrideValue(0);
            Assert.NotEqual(master.Value, card.Value);
            Assert.Equal(0, card.Value);
            Assert.Equal(master.Value, card.DefaultValue);
        }

        [Fact]
        public void Test_Effect_NotOverridden()
        {
            var master = new Innocence();
            var card = new InGameCard(master, new PlayerAccount("hoge"));
            Assert.Equal(master.Effect, card.Effect);
            Assert.Equal(master.Effect, card.DefaultEffect);
        }

        [Fact]
        public void Test_Efect_Overridden()
        {
            var master = new Innocence();
            var card = new InGameCard(master, new PlayerAccount("hoge"));
            var anotherMaster = new Overroid();
            card.OverrideEffect(anotherMaster.Effect);
            CustomAssertion.NotSameEffect(master.Effect, card.Effect);
            CustomAssertion.SameEffect(anotherMaster.Effect, card.Effect);
            CustomAssertion.SameEffect(master.Effect, card.DefaultEffect);
        }

        [Fact]
        public void Test_SetGuessed()
        {
            var master = new Innocence();
            var card = new InGameCard(master, new PlayerAccount("hoge"));
            card.SetGuessed();
            CustomAssertion.CardIsJustGuessable(card);
        }

        [Fact]
        public void Test_RevealByHack_FromHidden()
        {
            var master = new Innocence();
            var card = new InGameCard(master, new PlayerAccount("hoge"));
            card.RevealByHack();
            CustomAssertion.CardIsHacked(card);
        }

        [Fact]
        public void Test_RevealByHack_FromGuessed()
        {
            var master = new Innocence();
            var card = new InGameCard(master, new PlayerAccount("hoge"));
            card.SetGuessed();
            CustomAssertion.CardIsJustGuessable(card);
            card.RevealByHack();
            CustomAssertion.CardIsHacked(card);
        }

        [Fact]
        public void Test_SetGuessed_FromRevealed()
        {
            var master = new Innocence();
            var card = new InGameCard(master, new PlayerAccount("hoge"));
            card.RevealByHack();
            CustomAssertion.CardIsHacked(card);
            card.SetGuessed(); // still to be revealed
            CustomAssertion.CardIsHacked(card);
        }

        [Fact]
        public void Test_Open_FromHidden()
        {
            var master = new Innocence();
            var card = new InGameCard(master, new PlayerAccount("hoge"));
            card.Open();
            CustomAssertion.CardIsOpened(card);
        }

        [Fact]
        public void Test_Open_FromHacked()
        {
            var master = new Innocence();
            var card = new InGameCard(master, new PlayerAccount("hoge"));
            card.RevealByHack();
            CustomAssertion.CardIsHacked(card);
            card.Open();
            CustomAssertion.CardIsOpened(card);
        }

        [Fact]
        public void Test_ReturnDefault()
        {
            var master = new Innocence();
            var card = new InGameCard(master, new PlayerAccount("hoge"));
            var anotherMaster = new Overroid();

            card.OverrideValue(anotherMaster.Value);
            card.OverrideEffect(anotherMaster.Effect);
            
            Assert.NotEqual(master.Value, card.Value);
            CustomAssertion.NotSameEffect(master.Effect, card.Effect);

            card.ReturnToDefault();
            
            Assert.Equal(master.Value, card.Value);
            CustomAssertion.SameEffect(master.Effect, card.Effect);
        }

        [Fact]
        public void Test_IsVisibleTo_AlwaysVisibleToOwner()
        {
            var master = new Innocence();
            var p1 = new PlayerAccount("hoge");
            var card = new InGameCard(master, p1);

            Assert.True(card.IsVisibleTo(p1));
            var p2 = new PlayerAccount("fuga");
            Assert.False(card.IsVisibleTo(p2));
        }

        [Fact]
        public void Test_IsVisibleTo_TrueForAllPlayersWhenHacked()
        {
            var master = new Innocence();
            var p1 = new PlayerAccount("hoge");
            var card = new InGameCard(master, p1);
            card.RevealByHack();
            CustomAssertion.CardIsHacked(card);

            Assert.True(card.IsVisibleTo(p1));
            var p2 = new PlayerAccount("fuga");
            Assert.True(card.IsVisibleTo(p2));
        }

        [Fact]
        public void Test_IsVisibleTo_TrueForAllPlayersWhenOpened()
        {
            var master = new Innocence();
            var p1 = new PlayerAccount("hoge");
            var card = new InGameCard(master, p1);
            card.Open();
            CustomAssertion.CardIsOpened(card);

            Assert.True(card.IsVisibleTo(p1));
            var p2 = new PlayerAccount("fuga");
            Assert.True(card.IsVisibleTo(p2));
        }
    }
}
