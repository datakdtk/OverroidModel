using System.Collections.Generic;
using OverroidModel.Card.Master;

namespace OverroidModel.Game
{
    /// <summary>
    /// Interface to prepare shuffled card deck.
    /// </summary>
    interface ICardShuffler
    {
        /// <summary>
        /// Shuffle cards according to seed string,
        /// </summary>
        /// <param name="cards">List of cards used in the game.</param>
        /// <param name="seed">Parameter for randomizing card order.</param>
        /// <returns>Shuffled card deck.</returns>
        public List<ICardMaster> Shuffle(IEnumerable<ICardMaster> cards, string seed);
    }
}
