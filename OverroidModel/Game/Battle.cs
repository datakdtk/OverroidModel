using OverroidModel.Card;
using OverroidModel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OverroidModel.Game
{
    public class Battle
    {
        readonly ushort round;
        readonly PlayerAccount attackingPlayer;
        readonly PlayerAccount defendingPlayer;
        readonly Dictionary<PlayerAccount, InGameCard> cards;
        CardName? detectedCardName;
        PlayerAccount? winner;
        bool reversesCardValues;
        bool cardsOpened;

        public Battle(ushort round, PlayerAccount attackingPlayer, PlayerAccount defendingPlayer)
        {
            this.round = round;
            this.attackingPlayer = attackingPlayer;
            this.defendingPlayer = defendingPlayer;
            this.cards = new Dictionary<PlayerAccount, InGameCard>();
        }

        public ushort Round => round;
        public PlayerAccount AttackingPlayer => attackingPlayer;
        public PlayerAccount DefendingPlayer => defendingPlayer;
        public CardName? DetectedCardName => detectedCardName;
        public PlayerAccount? Winner => winner;

        public PlayerAccount PlayerOf(CardName cn)
        {
            if (cn == cards[AttackingPlayer]?.Name)
            {
                return AttackingPlayer;
            }
            if (cn == cards[DefendingPlayer]?.Name)
            {
                return DefendingPlayer;
            }
            throw new CardNotFoundException(
                String.Format("Failed to find battle card owner. cardName : {0}", cn)
            );
        }

        public InGameCard CardOf(PlayerAccount p)
        {
            if (!HasCardOf(p))
            {
                throw new CardNotFoundException("Player did not set a card on current battle round");
            }
            return cards[p];
        }

        public InGameCard CardOf(CardName cn)
        {
            InGameCard? card = cards.Values.Where(c => c.Name == cn).FirstOrDefault();
            if (card == null)
            {
                throw new CardNotFoundException(
                    String.Format("Card is not found in battle. cardName : {0}", cn)
                );
            }
            return card;
        }

        public bool HasCardOf(PlayerAccount p)
        {
            return cards.ContainsKey(p);
        }

        public bool HasBothPlayersCards()
        {
            return HasCardOf(attackingPlayer) && HasCardOf(defendingPlayer);
        }
        public bool CardsHaveOpened() => cardsOpened;

        public bool HasFinished() => winner != null;

        public ushort WinningStarOf(PlayerAccount p)
        {
            if (Winner == null || Winner != p)
            {
                return 0;
            }
            var loser = Winner == AttackingPlayer ? DefendingPlayer : AttackingPlayer;
            var detectionSuccess = CardOf(loser).Name == DetectedCardName;
            return detectionSuccess ? (ushort)2 : (ushort)1;
        }

        internal void SetCard(PlayerAccount p, InGameCard c, CardName? detectedCardName)
        {
            if (HasCardOf(p))
            {
                throw new UnavailableActionException("Battle Card has already been set.");
            }
            cards[p] = c;
            if (p == DefendingPlayer)
            {
                this.detectedCardName = detectedCardName;
            }
        }

        internal void ReplaceCard(PlayerAccount p, InGameCard c)
        {
            if (!HasCardOf(p))
            {
                throw new UnavailableActionException("Battle Card has not set yet.");
            }
            cards[p] = c;
        }

        internal void JudgeWinnerByValues()
        {
            if (Winner != null)
            {
                return; // Do not judge again if winner is already decided by effects.
            }
            var attackingCard = CardOf(AttackingPlayer);
            var defendingCard = CardOf(DefendingPlayer);
            // WARNING: It doesnot consider when card values are same
            bool attackerWins = reversesCardValues ? attackingCard.Value < defendingCard.Value : attackingCard.Value > defendingCard.Value;
            winner = attackerWins ? AttackingPlayer : DefendingPlayer;
        }

        internal void OpenCards() => cardsOpened = true;

        internal bool SetToReverseCardValues() => reversesCardValues = true;

        internal void SetSpecialWinner(CardName cn) => winner = PlayerOf(cn);
    }
}
