namespace Appian.Generator
{
    using System.Collections.Generic;
    using Appian.Domain;

    /// <summary>
    /// Provides an abstraction for generating a collection of cards to supply a deck.
    /// </summary>
    /// <remarks>
    /// The idea here is that it may be desirable to define the card collection in a remote data source - like a JSON
    /// file or data store - and pass a generator that knows how to access that data to a general Deck implementation.
    /// This way, clients could define their own cards without having to develop a new subclass.
    /// </remarks>
    public interface ICardGenerator
    {
        /// <summary>
        /// Gets the raw array of cards.
        /// </summary>
        /// <returns>An array of <see cref="Card"/>.</returns>
        Card[] GetCardArray();

        /// <summary>
        /// Gets the array of cards converted into an <see cref="IReadOnlyCollection{T}"/> type supported by the generator.
        /// </summary>
        /// <returns>The converted collection of cards.</returns>
        IReadOnlyCollection<Card> GetCards();

        /// <summary>
        /// Converts an array of cards into the <see cref="IReadOnlyCollection{T}"/> type supported by the generator.
        /// </summary>
        /// <param name="cards">The cards to be converted.</param>
        /// <returns>The converted <see cref="IReadOnlyCollection{T}"/>.</returns>
        IReadOnlyCollection<Card> Convert(IEnumerable<Card> cards);
    }
}