using OverroidModel.Card;
using OverroidModel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OverroidModel.Game
{
    /// <summary>
    /// Hand cards of a game player,
    /// </summary>
    public class PlayerHand
    {
        readonly List<InGameCard> cards;

        /// <param name="cards">Cards in the hand.</param>
        public PlayerHand(List<InGameCard> cards)
        {
            this.cards = cards;
        }

        /// <summary>
        /// Cards in the hand.
        /// </summary>
        public IReadOnlyList<InGameCard> Cards => cards;

        /// <summary>
        /// The number of all cards.
        /// </summary>
        public ushort Count => (ushort)Cards.Count;

        /// <summary>
        /// The number of cards that are not revealed.
        /// </summary>
        public ushort UnrevealedCardCount => (ushort)cards.Where(c => c.Visibility != CardVisibility.Hacked).Count();

        /// <summary>
        /// Names of cards that are guessable by the opponent.
        /// </summary>
        public IReadOnlyList<CardName> GuessableCardNames => cards.Where(c => c.Visibility != CardVisibility.Hidden).Select(c => c.Name).ToList();

        /// <summary>
        /// Try to get card with given name.
        /// </summary>
        /// <returns>returns null if named card is not in the hand.</returns>
        public InGameCard? CardOf(CardName cn) => cards.Where(c => c.Name == cn).FirstOrDefault();

        /// <summary>
        /// Check if a card with given name is in the hand.
        /// </summary>
        public bool HasCard(CardName cn) => CardOf(cn) != null;

        /// <summary>
        /// Remove a card with given name from the hand.
        /// </summary>
        /// <returns>Removed card.</returns>
        /// <exception cref="UnavailableActionException">Thrown if a card with given name is not in the hand.</exception>
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

        /// <summary>
        /// Add a card to the hand.
        /// Value and effect of the card to add return to default.
        /// </summary>
        /// <param name="c">Card to add</param>
        internal void AddCard(InGameCard c) {
            c.ReturnToDefault();
            c.SetGuessed();
            cards.Add(c);
        }

        /// <summary>
        /// Selects an unrevealed card at random and returns it.
        /// </summary>
        /// <returns>Removed card.</returns>
        /// <exception cref="UnavailableActionException">Thrown when there is no unrevealed card in the hand.</exception>
        internal InGameCard SelectRandamUnrevealCard()
        {
            var unrevealedCards = cards.FindAll(c => c.Visibility != CardVisibility.Hacked);
            if (unrevealedCards.Count == 0)
            {
                throw new UnavailableActionException("unrevealed card does not exist. so cannot choose at random");
            }
            var rand = new Random();
            var randomIndex = rand.Next(unrevealedCards.Count - 1);
            return unrevealedCards[randomIndex];
        }

    }
}
