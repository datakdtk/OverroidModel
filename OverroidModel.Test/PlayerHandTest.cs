using OverroidModel.Card;
using OverroidModel.Card.Master;
using OverroidModel.Exceptions;
using OverroidModel.Test.TestLib;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OverroidModel.Test
{
    public class PlayerHandTest
    {
        [Fact]
        public void Test_Count()
        {
            var player = new PlayerAccount("hoge");
            var cards = GetCardList(player);
            cards[2].SetGuessed();
            cards[3].SetGuessed();
            cards[4].RevealByHack();
            cards[5].RevealByHack();
            var hand = new PlayerHand(player, cards);
            Assert.Equal(cards.Count, hand.Count);
        }

        [Fact]
        public void Test_UnrevealedCardCount()
        {
            var player = new PlayerAccount("hoge");
            var cards = GetCardList(player);
            cards[2].SetGuessed();
            cards[3].SetGuessed();
            cards[4].RevealByHack();
            cards[5].RevealByHack();
            var hand = new PlayerHand(player, cards);
            Assert.Equal(cards.Count - 2, hand.UnrevealedCardCount);
        }

        [Fact]
        public void Test_GuessableCardNames()
        {
            var player = new PlayerAccount("hoge");
            var cards = GetCardList(player);
            cards[2].SetGuessed();
            cards[3].SetGuessed();
            cards[4].RevealByHack();
            cards[5].RevealByHack();
            var expected = cards.ToArray()[2..6].Select(c => c.Name).OrderBy(n => n).ToArray();

            var hand = new PlayerHand(player, cards);
            var actual = hand.GuessableCardNames.OrderBy(n => n).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_CardOf_Found()
        {
            var player = new PlayerAccount("hoge");
            var cards = GetCardList(player);
            var hand = new PlayerHand(player, cards);

            var name = CardName.Creator;
            var c = hand.CardOf(name);
            Assert.NotNull(c);
            Assert.Equal(name, c?.Name);
        }

        [Fact]
        public void Test_CardOf_NotFound()
        {
            var player = new PlayerAccount("hoge");
            var cards = GetCardList(player);
            var hand = new PlayerHand(player, cards);

            var name = CardName.Overroid;
            var c = hand.CardOf(name);
            Assert.Null(c);
        }

        [Fact]
        public void Test_HasCard_True()
        {
            var player = new PlayerAccount("hoge");
            var cards = GetCardList(player);
            var hand = new PlayerHand(player, cards);

            var name = CardName.Hacker;
            Assert.True(hand.HasCard(name));
        }

        [Fact]
        public void Test_HasCard_False()
        {
            var player = new PlayerAccount("hoge");
            var cards = GetCardList(player);
            var hand = new PlayerHand(player, cards);

            var name = CardName.Overroid;
            Assert.False(hand.HasCard(name));
        }

        [Fact]
        public void Test_AddCard_CardIsAdded()
        {
            var player = new PlayerAccount("hoge");
            var cards = GetCardList(player);
            var hand = new PlayerHand(player, cards);

            var additionalCard = new InGameCard(new Overroid(), player);
            Assert.Equal(6, hand.Count);
            Assert.False(hand.HasCard(additionalCard.Name));

            hand.AddCard(additionalCard);
            Assert.Equal(7, hand.Count);
            Assert.True(hand.HasCard(additionalCard.Name));
        }

        [Fact]
        public void Test_AddCard_CardReturnsToDefault()
        {
            var player = new PlayerAccount("hoge");
            var cards = GetCardList(player);
            var hand = new PlayerHand(player, cards);
            var additionalCard = new InGameCard(new Overroid(), player);

            var anotherCard = new Death();
            additionalCard.OverrideValue(99);
            additionalCard.OverrideEffect(anotherCard.Effect);

            hand.AddCard(additionalCard);
            var cardInHand = hand.CardOf(additionalCard.Name);
            Assert.NotNull(cardInHand);

            Assert.Equal(cardInHand?.Value, cardInHand?.DefaultValue);
            Assert.IsType(cardInHand?.Effect.GetType(), cardInHand?.DefaultEffect);
        }

        [Fact]
        public void Test_AddCard_CardIsGuessable()
        {
            var player = new PlayerAccount("hoge");
            var cards = GetCardList(player);
            var hand = new PlayerHand(player, cards);
            var additionalCard = new InGameCard(new Overroid(), player);

            hand.AddCard(additionalCard);
            var cardInHand = hand.CardOf(additionalCard.Name);
            Assert.NotNull(cardInHand);

            CustomAssertion.CardIsJustGuessable(cardInHand!);
        }

        [Fact]
        public void Test_AddCard_CardKeepsRevealedIfHasBeenHacked()
        {
            var player = new PlayerAccount("hoge");
            var cards = GetCardList(player);
            var hand = new PlayerHand(player, cards);
            var additionalCard = new InGameCard(new Overroid(), player);
            additionalCard.RevealByHack();

            hand.AddCard(additionalCard);
            var cardInHand = hand.CardOf(additionalCard.Name);
            Assert.NotNull(cardInHand);

            CustomAssertion.CardIsHacked(cardInHand!);
        }

        [Fact]
        public void Test_AddCard_CardTurnsGuessedIfHasBeenOpend()
        {
            var player = new PlayerAccount("hoge");
            var cards = GetCardList(player);
            var hand = new PlayerHand(player, cards);
            var additionalCard = new InGameCard(new Overroid(), player);
            additionalCard.Open();
            CustomAssertion.CardIsOpened(additionalCard);

            hand.AddCard(additionalCard);
            var cardInHand = hand.CardOf(additionalCard.Name);
            Assert.NotNull(cardInHand);

            CustomAssertion.CardIsJustGuessable(cardInHand!);
        }

        [Fact]
        public void Test_RemoveCard_CardIsRemoved()
        {
            var player = new PlayerAccount("hoge");
            var cards = GetCardList(player);
            var hand = new PlayerHand(player, cards);
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
            var player = new PlayerAccount("hoge");
            var cards = GetCardList(player);
            var hand = new PlayerHand(player, cards);
            var cn = CardName.Doctor;

            var Removed = hand.RemoveCard(cn);

            Assert.Equal(cn, Removed.Name);
        }

        [Fact]
        public void Test_RemoveCard_ErrorIfNotExist()
        {
            var player = new PlayerAccount("hoge");
            var cards = GetCardList(player);
            var hand = new PlayerHand(player, cards);
            var cn = CardName.Overroid;

            Assert.Throws<GameLogicException>(() => hand.RemoveCard(cn));
        }

        [Fact]
        public void Test_SelectRadomUnrevealedLand_SelectedCardIsStillInHand()
        {
            var player = new PlayerAccount("hoge");
            var cards = GetCardList(player);
            var hand = new PlayerHand(player, cards);
            var selected = hand.SelectRandomUnrevealCard();

            Assert.True(hand.HasCard(selected.Name));
        }


        [Fact]
        public void Test_SelectRadomUnrevealedLand_RevealedCardNeverSelected()
        {
            var player = new PlayerAccount("hoge");
            var cards = new List<InGameCard>()
            {
                CardDictionary.GetInGameCard(CardName.Innocence, player),
                CardDictionary.GetInGameCard(CardName.Hacker, player),
                CardDictionary.GetInGameCard(CardName.Creator, player),
            };
            cards[0].RevealByHack();
            var hand = new PlayerHand(player, cards);
            var selected = hand.SelectRandomUnrevealCard();

            Assert.True(selected.Name == CardName.Hacker || selected.Name == CardName.Creator, selected.Name.ToString());
        }

        [Fact]
        public void Test_SelectRadomUnrevealedLand_GuessedCardMayBeSelected()
        {
            var player = new PlayerAccount("hoge");
            var cards = new List<InGameCard>()
            {
                CardDictionary.GetInGameCard(CardName.Innocence, player),
                CardDictionary.GetInGameCard(CardName.Hacker, player),
                CardDictionary.GetInGameCard(CardName.Creator, player),
            };
            cards[0].RevealByHack();
            cards[1].SetGuessed();
            cards[2].SetGuessed();
            var hand = new PlayerHand(player, cards);
            var selected = hand.SelectRandomUnrevealCard();

            Assert.True(selected.Name == CardName.Hacker || selected.Name == CardName.Creator, selected.Name.ToString());
        }

        [Fact]
        public void Test_SelectRadomUnrevealedLand_ThrownIfAllCardsAreRevealed()
        {
            var player = new PlayerAccount("hoge");
            var cards = new List<InGameCard>()
            {
                CardDictionary.GetInGameCard(CardName.Innocence, player),
                CardDictionary.GetInGameCard(CardName.Hacker, player),
                CardDictionary.GetInGameCard(CardName.Creator, player),
            };
            cards[0].RevealByHack();
            cards[1].RevealByHack();
            cards[2].RevealByHack();
            var hand = new PlayerHand(player, cards);
            Assert.Throws<UnavailableActionException>(() => hand.SelectRandomUnrevealCard());
        }

        private static List<InGameCard> GetCardList(PlayerAccount p)
        {
            return new List<InGameCard>()
            {
                CardDictionary.GetInGameCard(CardName.Innocence, p),
                CardDictionary.GetInGameCard(CardName.Hacker, p),
                CardDictionary.GetInGameCard(CardName.Creator, p),
                CardDictionary.GetInGameCard(CardName.Doctor, p),
                CardDictionary.GetInGameCard(CardName.Idol, p),
                CardDictionary.GetInGameCard(CardName.Trickster, p),
            };
        }
    }
}
