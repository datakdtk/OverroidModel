using OverroidModel.Card;
using OverroidModel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OverroidModel
{
    /// <summary>
    /// Hand cards of a game player,
    /// </summary>
    public class PlayerHand
    {
        readonly List<InGameCard> cards;
        readonly PlayerAccount player;

        /// <param name="player">Cards in the hand.</param>
        /// <param name="cards">Player who uses cards in the hand.</param>
        public PlayerHand(PlayerAccount player, IEnumerable<InGameCard> cards)
        {
            this.player = player;
            this.cards = cards.ToList();
        }

        /// <summary>
        /// Cards in the hand.
        /// </summary>
        public IReadOnlyList<InGameCard> Cards => cards;

        /// <summary>
        /// Player who uses the cards
        /// </summary>
        public PlayerAccount Player => player;

        /// <summary>
        /// The number of all cards.
        /// </summary>
        public ushort Count => (ushort)Cards.Count;

        /// <summary>
        /// The number of cards that are not revealed.
        /// </summary>
        public ushort UnrevealedCardCount => (ushort)cards.Where( c => !c.IsHacked() ).Count();

        /// <summary>
        /// Names of cards that are guessable by the opponent.
        /// </summary>
        public IReadOnlyList<CardName> GuessableCardNames => cards.Where( c => c.IsGuessable() ).Select(c => c.Name).ToList();

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
        /// Selects an card at random including revealed cards.
        /// </summary>
        /// <returns>Selected card.</returns>
        /// <exception cref="UnavailableActionException">Thrown when there is no card in the hand.</exception>
        public InGameCard SelectRandomCard()
        {
            if (Count == 0)
            {
                throw new UnavailableActionException("Card does not exist in hand. So cannot choose at random");
            }
            var rand = new Random();
            var randomIndex = rand.Next(Count - 1);
            return cards[randomIndex];
        }

        /// <summary>
        /// Selects an unrevealed card at random.
        /// </summary>
        /// <returns>Selected card.</returns>
        /// <exception cref="UnavailableActionException">Thrown when there is no unrevealed card in the hand.</exception>
        public InGameCard SelectRandomUnrevealCard()
        {
            var unrevealedCards = cards.FindAll( c => !c.IsHacked() );
            if (unrevealedCards.Count == 0)
            {
                throw new UnavailableActionException("Unrevealed card does not exist. So cannot choose at random");
            }
            var rand = new Random();
            var randomIndex = rand.Next(unrevealedCards.Count - 1);
            return unrevealedCards[randomIndex];
        }

        /// <summary>
        /// Remove a card with given name from the hand.
        /// </summary>
        /// <returns>Removed card.</returns>
        /// <exception cref="GameLogicException">Thrown if a card with given name is not in the hand.</exception>
        internal InGameCard RemoveCard(CardName cn)
        {
            var c = CardOf(cn);
            if (c == null)
            {
                throw new GameLogicException("Card cannot remove from hand");
            }
            cards.Remove(c);
            return c;
        }

        /// <summary>
        /// Add a card to the hand.
        /// Value and effect of the card to add return to default.
        /// </summary>
        /// <param name="c">Card to add</param>
        internal void AddCard(InGameCard c)
        {
            if (c.Owner != player)
            {
                throw new GameLogicException("Card is tried to add to hand of non-owner player");
            }
            c.ReturnToDefault();
            c.SetGuessed();
            cards.Add(c);
        }

    }
}
