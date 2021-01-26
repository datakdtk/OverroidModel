using System.Collections.Generic;
using OverroidModel.Card.Master;
using OverroidModel.Exceptions;

namespace OverroidModel.Card
{
    /// <summary>
    /// Card that is not used in the game. 
    /// </summary>
    public class OutsideCard
    {
        readonly ICardMaster card;
        readonly List<PlayerAccount> lookingPlayers;

        /// <param name="card">Data of the card.</param>
        public OutsideCard(ICardMaster card)
        {
            this.card = card;
            lookingPlayers = new List<PlayerAccount>();
        }

        /// <summary>
        /// Check if player can see what the card is.
        /// </summary>
        /// <param name="player">Player trying to see the card.</param>
        /// <returns>Returns true if the player can see the card.</returns>
        public bool IsViewableTo(PlayerAccount player)
        {
            return lookingPlayers.Contains(player);
        }

        /// <summary>
        /// Get card data if available by given player
        /// </summary>
        /// <param name="player">Player trying to see the card.</param>
        /// <returns>Card data,</returns>
        /// <exception cref="UnavaibleActionException">Thrown when the player is not authorized to look at the card.</exception>
        public ICardMaster LookedAtBy(PlayerAccount player)
        {
            if (!IsViewableTo(player))
            {
                throw new UnavailableActionException("The card is not viewable to given player");
            }
            return card;
        }

        /// <summary>
        /// Make given player be able to look at the card
        /// </summary>
        /// <param name="player">Player who gets access to the card.</param>
        internal void RevealTo(PlayerAccount player)
        {
            lookingPlayers.Add(player);
        }
    }
}
