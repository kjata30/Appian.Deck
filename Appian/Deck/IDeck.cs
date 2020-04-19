using Appian.Domain;

namespace Appian.Deck
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides an abstraction for a deck the contains cards of some type and methods to operate on those cards,
    /// including shuffling and dealing.
    /// </summary>
    public interface IDeck
    {
        /// <summary>
        /// Shuffles the deck, randomizing the order of its cards.
        /// </summary>
        void Shuffle();

        /// <summary>
        /// Deals (removes and returns) the provided number of cards from the deck, if possible.
        /// </summary>
        /// <param name="amount">The amount of cards to deal.</param>
        /// <returns>The number of cards requested from the deck.</returns>
        IEnumerable<Card> Deal(int amount);

        /// <summary>
        /// Deals (removes and returns) one card from the deck, if possible.
        /// </summary>
        /// <returns>One card from the deck.</returns>
        Card DealOneCard();
    }
}