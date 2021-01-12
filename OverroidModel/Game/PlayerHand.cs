using OverroidModel.Card;
using OverroidModel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OverroidModel.Game
{
    public class PlayerHand
    {
        readonly List<InGameCard> cards;

        public PlayerHand(List<InGameCard> cards)
        {
            this.cards = cards;
        }

        public IReadOnlyList<InGameCard> Cards => cards;
        public ushort Count => (ushort)Cards.Count;
        public ushort UnrevealedCardCount => (ushort)(Count - GuessableCardNames.Count);
        public IReadOnlyList<CardName> GuessableCardNames => cards.Where(c => c.Visibility != CardVisibility.Hidden).Select(c => c.Name).ToList();
        public IReadOnlyList<InGameCard> RevealedCards => cards.FindAll(c => c.Visibility == CardVisibility.Revealed);

        public InGameCard? CardOf(CardName cn) => cards.Where(c => c.Name == cn).FirstOrDefault();

        public bool HasCard(CardName cn) => CardOf(cn) != null;

        internal InGameCard RemoveCard(CardName cn)
        {
            var c = CardOf(cn);
            if (c == null)
            {
                throw new UnavailableActionException("Card cannot remove from hand");
            }
            cards.Remove(c);
            return c;
        }

        internal void AddCard(InGameCard c) {
            c.ReturnToDefault();
            cards.Add(c);
        }

        internal InGameCard RemoveRandomUnrevealedCard() => RemoveCard(GetRandamUnrevealCard().Name);

        internal void RevealRandomCard() => GetRandamUnrevealCard().Reveal();

        private InGameCard GetRandamUnrevealCard()
        {
            var unrevealedCards = cards.FindAll(c => c.Visibility != CardVisibility.Revealed);
            if (unrevealedCards.Count == 0)
            {
                throw new UnavailableActionException("unrevealed card does not exist. so cannot choose at random");
            }
            var rand = new Random();
            var randomIndex = rand.Next(unrevealedCards.Count - 1);
            return cards[randomIndex];
        }
    }
}
