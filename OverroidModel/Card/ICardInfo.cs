using System;
using System.Collections.Generic;
using System.Text;

namespace OverroidModel.Card
{
    interface ICardInfo
    {
        /// <summary>
        /// Card name that identifies each card in game.
        /// </summary>
        public CardName Name { get; }

        /// <summary>
        /// Check if the card is face up.
        /// </summary>
        public bool IsOpened();

        /// <summary>
        /// Check if player can see what the card is.
        /// </summary>
        /// <param name="player">Player trying to see the card.</param>
        /// <returns>
        /// Returns true if the player can see the card.
        /// When the card is opened, it must return true for all players.
        /// </returns>
        public bool IsVisibleTo(PlayerAccount player);

    }
}
