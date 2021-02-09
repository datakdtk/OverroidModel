﻿using System.Collections.Generic;
using OverroidModel.Card.Master;

namespace OverroidModel.Card
{
    /// <summary>
    /// Card that is not used in the game. 
    /// </summary>
    public class OutsideCard : ICardInfo
    {
        readonly ICardMaster card;
        readonly List<PlayerAccount> lookingPlayers;
        bool isOpened = false;

        /// <param name="card">Data of the card.</param>
        public OutsideCard(ICardMaster card)
        {
            this.card = card;
            lookingPlayers = new List<PlayerAccount>();
        }

        public CardName Name => card.Name;

        public bool IsOpened() => isOpened;

        public bool IsVisibleTo(PlayerAccount player)
        {
            return isOpened || lookingPlayers.Contains(player);
        }

        /// <summary>
        /// Make given player be able to look at the card
        /// </summary>
        /// <param name="player">Player who gets access to the card.</param>
        internal void RevealTo(PlayerAccount player)
        {
            lookingPlayers.Add(player);
        }

        /// <summary>
        /// Make the card face up and reveal to all players
        /// </summary>
        internal void Open() => isOpened = true;
    }
}
