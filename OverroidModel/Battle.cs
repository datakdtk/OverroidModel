using OverroidModel.Card;
using OverroidModel.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace OverroidModel
{
    /// <summary>
    /// Battle of cards by comparing their values.
    /// </summary>
    public class Battle
    {
        readonly ushort round;
        readonly PlayerAccount attackingPlayer;
        readonly PlayerAccount defendingPlayer;
        readonly Dictionary<PlayerAccount, InGameCard> cards;
        CardName? detectedCardName;
        PlayerAccount? winner;
        bool hasFinished;
        bool reversesCardValues;

        /// <param name="round">Current battle number</param>
        /// <param name="attackingPlayer">Player who places battle card first,</param>
        /// <param name="defendingPlayer">Player who places battle card second.</param>
        public Battle(ushort round, PlayerAccount attackingPlayer, PlayerAccount defendingPlayer)
        {
            this.round = round;
            this.attackingPlayer = attackingPlayer;
            this.defendingPlayer = defendingPlayer;
            cards = new Dictionary<PlayerAccount, InGameCard>();
        }

        /// <summary>
        /// Current battle number.
        /// </summary>
        public ushort Round => round;

        /// <summary>
        /// Player who places battle card first.
        /// </summary>
        public PlayerAccount AttackingPlayer => attackingPlayer;

        /// <summary>
        /// Player who places battle card second.
        /// </summary>
        public PlayerAccount DefendingPlayer => defendingPlayer;

        /// <summary>
        /// Card name detected by defending player if he did.
        /// </summary>
        public CardName? DetectedCardName => detectedCardName;

        /// <summary>
        /// Owner of the card that won this battle.
        /// </summary>
        public PlayerAccount? Winner => winner;

        /// <summary>
        /// Get Owner of the named card that is placed in this battle.
        /// </summary>
        /// <param name="cn">Name of one of the cards that are placed in this game.</param>
        /// <exception cref="CardNotFoundException">Thrown when named card is not placed in this battle.</exception>
        public PlayerAccount PlayerOf(CardName cn)
        {
            if (HasCardOf(attackingPlayer) && cn == cards[AttackingPlayer].Name)
            {
                return AttackingPlayer;
            }
            if (HasCardOf(defendingPlayer) && cn == cards[DefendingPlayer].Name)
            {
                return DefendingPlayer;
            }
            throw new CardNotFoundException(
                string.Format("Failed to find battle card owner. cardName : {0}", cn)
            );
        }

        /// <summary>
        /// Get the card placed by given player.
        /// </summary>
        /// <param name="p">one of the game players.</param>
        /// <exception cref="CardNotFoundException">Thrown when given player has not placed a card yet..</exception>
        public InGameCard CardOf(PlayerAccount p)
        {
            if (!HasCardOf(p))
            {
                throw new CardNotFoundException($"Player did not set a card in round {Round}");
            }
            return cards[p];
        }

        /// <summary>
        /// Get the Card with given name that is placed in this battle.
        /// </summary>
        /// <param name="cn">Name of one of the cards that are placed in this game.</param>
        /// <exception cref="CardNotFoundException">Thrown when named card is not placed in this battle.</exception>
        public InGameCard CardOf(CardName cn)
        {
            var card = cards.Values.Where(c => c.Name == cn).FirstOrDefault();
            if (card == null)
            {
                throw new CardNotFoundException(
                    string.Format("Card is not found in battle. cardName : {0}", cn)
                );
            }
            return card;
        }

        /// <summary>
        /// Check if named card has been set in this battle.
        /// </summary>
        /// <param name="p">Name of a card to check.</param>
        public bool HasCardOf(PlayerAccount p)
        {
            return cards.ContainsKey(p);
        }

        /// <summary>
        /// Check if the battle has already decided its winner.
        /// Additional card effects might be triggered even if this method returns true.
        /// </summary>
        public bool HasFinished() => hasFinished;

        /// <summary>
        /// Returns true if battle has ended and there is no winner. 
        /// </summary>
        public bool IsDrawBattle() => hasFinished && winner == null;

        /// <summary>
        /// Get number of stars that given player got in this battle.
        /// </summary>
        /// <param name="p">A Player in this game.</param>
        /// <exception cref="NonGamePlayerException"></exception>
        public ushort WinningStarOf(PlayerAccount p)
        {
            if (p != attackingPlayer && p != defendingPlayer)
            {
                throw new NonGamePlayerException(p);
            }

            if (Winner == null || Winner != p)
            {
                return 0;
            }

            var detectionSuccess = Winner == DefendingPlayer && CardOf(AttackingPlayer).Name == DetectedCardName;
            return (ushort)(detectionSuccess ? 2 : 1);
        }

        /// <summary>
        /// Set a card to this battle.
        /// </summary>
        /// <param name="c">Card to be placed.</param>
        internal void SetCard(InGameCard c) => cards[c.Owner] = c;

        /// <summary>
        /// Set card name for detection if defending player chose to detect.
        /// </summary>
        internal void DetectCard(CardName? detectedCardName) => this.detectedCardName = detectedCardName;

        /// <summary>
        /// Compares card values and determines the battle winner if not determined yet.
        /// </summary>
        /// <exception cref="CardNotFoundException">Thrown if any player did not set a card..</exception>
        internal void Finish()
        {
            if (!HasCardOf(AttackingPlayer) || !HasCardOf(DefendingPlayer))
            {
                throw new GameLogicException("Can not judge the battle winner. Not all cards have been set");
            }
            hasFinished = true;
            var attackingCard = CardOf(AttackingPlayer);
            var defendingCard = CardOf(DefendingPlayer);

            if (winner != null || attackingCard.Value == defendingCard.Value)
            {
                return;
            }

            var attackerWins = reversesCardValues ? attackingCard.Value < defendingCard.Value : attackingCard.Value > defendingCard.Value;
            winner = attackerWins ? AttackingPlayer : DefendingPlayer;
        }

        /// <summary>
        /// Changes the rule of this battle from greater-wins to lesser-wins.
        /// </summary>
        internal void SetToReverseCardValues() => reversesCardValues = true;

        /// <summary>
        /// Set a card that wins by its effect.
        /// </summary>
        /// <param name="cn">Name of card to win,</param>
        internal void SetSpecialWinner(CardName cn) => winner = PlayerOf(cn);

    }
}
