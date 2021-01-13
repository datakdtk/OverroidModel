﻿using OverroidModel.Card;
using OverroidModel.Card.Master;
using OverroidModel.Exceptions;
using OverroidModel.Game;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OverroidModel.Test.Game
{
    public class PlayerHandTest
    {
        [Fact]
        public void Test_Count()
        {
            var cards = GetCardList();
            cards[2].SetGuessed();
            cards[3].SetGuessed();
            cards[4].Reveal();
            cards[5].Reveal();
            var hand = new PlayerHand(cards);
            Assert.Equal(cards.Count, hand.Count);
        }

        [Fact]
        public void Test_UnrevealedCardCount()
        {
            var cards = GetCardList();
            cards[2].SetGuessed();
            cards[3].SetGuessed();
            cards[4].Reveal();
            cards[5].Reveal();
            var hand = new PlayerHand(cards);
            Assert.Equal(cards.Count - 2, hand.UnrevealedCardCount);
        }

        [Fact]
        public void Test_GuessableCardNames()
        {
            var cards = GetCardList();
            cards[2].SetGuessed();
            cards[3].SetGuessed();
            cards[4].Reveal();
            cards[5].Reveal();
            var expected = cards.ToArray()[2..6].Select(c => c.Name).OrderBy(n => n).ToArray();
            var hand = new PlayerHand(cards);
            var actual = hand.GuessableCardNames.OrderBy(n => n).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_CardOf_Found()
        {
            var cards = GetCardList();
            var hand = new PlayerHand(cards);
            var name = CardName.Creator;
            var c = hand.CardOf(name);
            Assert.NotNull(c);
            Assert.Equal(name, c?.Name);
        }

        [Fact]
        public void Test_CardOf_NotFound()
        {
            var cards = GetCardList();
            var hand = new PlayerHand(cards);
            var name = CardName.Overroid;
            var c = hand.CardOf(name);
            Assert.Null(c);
        }

        [Fact]
        public void Test_HasCard_True()
        {
            var cards = GetCardList();
            var hand = new PlayerHand(cards);
            var name = CardName.Hacker;
            Assert.True(hand.HasCard(name));
        }

        [Fact]
        public void Test_HasCard_False()
        {
            var cards = GetCardList();
            var hand = new PlayerHand(cards);
            var name = CardName.Overroid;
            Assert.False(hand.HasCard(name));
        }

        [Fact]
        public void Test_AddCard_CardIsAdded()
        {
            var cards = GetCardList();
            var hand = new PlayerHand(cards);
            var additionalCard = new InGameCard(new Overroid());
            Assert.Equal(6, hand.Count);
            Assert.False(hand.HasCard(additionalCard.Name));

            hand.AddCard(additionalCard);
            Assert.Equal(7, hand.Count);
            Assert.True(hand.HasCard(additionalCard.Name));
        }

        [Fact]
        public void Test_AddCard_CardReturnsToDefault()
        {
            var cards = GetCardList();
            var hand = new PlayerHand(cards);
            var additionalCard = new InGameCard(new Overroid());

            var anotherCard = new Death();
            additionalCard.OverrideValue(99);
            additionalCard.OverrideEffect(anotherCard.Effect);

            hand.AddCard(additionalCard);
            var cardInHand = hand.CardOf(additionalCard.Name);
            Assert.NotNull(cardInHand);

            Assert.Equal(cardInHand?.Value, cardInHand?.DefaultValue);
            Assert.Equal(cardInHand?.Effect, cardInHand?.DefaultEffect);
        }

        [Fact]
        public void Test_AddCard_CardIsGuessable()
        {
            var cards = GetCardList();
            var hand = new PlayerHand(cards);
            var additionalCard = new InGameCard(new Overroid());

            hand.AddCard(additionalCard);
            var cardInHand = hand.CardOf(additionalCard.Name);
            Assert.NotNull(cardInHand);

            Assert.Equal(CardVisibility.Guessed, cardInHand?.Visibility);
        }

        [Fact]
        public void Test_AddCard_CardKeepsRevealed()
        {
            var cards = GetCardList();
            var hand = new PlayerHand(cards);
            var additionalCard = new InGameCard(new Overroid());
            additionalCard.Reveal();

            hand.AddCard(additionalCard);
            var cardInHand = hand.CardOf(additionalCard.Name);
            Assert.NotNull(cardInHand);

            Assert.Equal(CardVisibility.Revealed, cardInHand?.Visibility);
        }

        [Fact]
        public void Test_RemoveCard_CardIsRemoved()
        {
            var cards = GetCardList();
            var hand = new PlayerHand(cards);
            var cn = CardName.Doctor;

            Assert.Equal(6, hand.Count);
            Assert.True(hand.HasCard(cn));

            hand.RemoveCard(cn);

            Assert.Equal(5, hand.Count);
            Assert.False(hand.HasCard(cn));
        }

        [Fact]
        public void Test_RemoveCard_ReturnsRemovedCard()
        {
            var cards = GetCardList();
            var hand = new PlayerHand(cards);
            var cn = CardName.Doctor;

            var Removed = hand.RemoveCard(cn);

            Assert.Equal(cn, Removed.Name);
        }

        [Fact]
        public void Test_RemoveCard_ErrorIfNotExist()
        {
            var cards = GetCardList();
            var hand = new PlayerHand(cards);
            var cn = CardName.Overroid;

            Assert.Throws<UnavailableActionException>(() => hand.RemoveCard(cn));
        }

        [Fact]
        public void Test_SelectRadomUnrevealedLand_SelectedCardIsStillInHand()
        {
            var cards = GetCardList();
            var hand = new PlayerHand(cards);
            var selected = hand.SelectRandamUnrevealCard();

            Assert.True(hand.HasCard(selected.Name));
        }


        [Fact]
        public void Test_SelectRadomUnrevealedLand_RevealedCardNeverSelected()
        {
            var cards =  new List<InGameCard>()
            {
                new InGameCard(new Innocence()),
                new InGameCard(new Hacker()),
                new InGameCard(new Creator()),
            };
            cards[0].Reveal();
            var hand = new PlayerHand(cards);
            var selected = hand.SelectRandamUnrevealCard();

            Assert.True(selected.Name == CardName.Hacker || selected.Name == CardName.Creator, selected.Name.ToString());
        }

        [Fact]
        public void Test_SelectRadomUnrevealedLand_GuessedCardMayBeSelected()
        {
            var cards =  new List<InGameCard>()
            {
                new InGameCard(new Innocence()),
                new InGameCard(new Hacker()),
                new InGameCard(new Creator()),
            };
            cards[0].Reveal();
            cards[1].SetGuessed();
            cards[2].SetGuessed();
            var hand = new PlayerHand(cards);
            var selected = hand.SelectRandamUnrevealCard();

            Assert.True(selected.Name == CardName.Hacker || selected.Name == CardName.Creator, selected.Name.ToString());
        }

        [Fact]
        public void Test_SelectRadomUnrevealedLand_ThrownIfAllCardsAreRevealed()
        {
            var cards =  new List<InGameCard>()
            {
                new InGameCard(new Innocence()),
                new InGameCard(new Hacker()),
                new InGameCard(new Creator()),
            };
            cards[0].Reveal();
            cards[1].Reveal();
            cards[2].Reveal();
            var hand = new PlayerHand(cards);
            Assert.Throws<UnavailableActionException>(() => hand.SelectRandamUnrevealCard());
        }

        private static List<InGameCard> GetCardList()
        {
            return new List<InGameCard>()
            {
                new InGameCard(new Innocence()),
                new InGameCard(new Hacker()),
                new InGameCard(new Creator()),
                new InGameCard(new Doctor()),
                new InGameCard(new Idol()),
                new InGameCard(new Trickster()),
            };
        }
    }
}
