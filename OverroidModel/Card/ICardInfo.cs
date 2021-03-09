using OverroidModel.Card.Effects;

namespace OverroidModel.Card
{
    public interface ICardInfo
    {
        /// <summary>
        /// Card name that identifies each card in game.
        /// </summary>
        public CardName Name { get; }

        /// <summary>
        /// Strength of the card.
        /// </summary>
        public ushort Value { get; }

        /// <summary>
        /// Unique effect of the cards.
        /// </summary>
        public ICardEffect Effect { get; }

        /// <summary>
        /// Player using this card. Returns null if it is not used in the game.
        /// </summary>
        public PlayerAccount? Owner { get; }

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

        /// <summary>
        /// Check if a player can guess his opponent has the card.
        /// </summary>
        public bool IsGuessable();
    }
}
