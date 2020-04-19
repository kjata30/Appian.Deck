namespace Appian.Generator.Implementation
{
    using System.Collections.Generic;
    using System.Linq;
    using Appian.Domain;

    /// <summary>
    /// An in-memory card generator for a standard deck of playing cards.
    /// </summary>
    /// <inheritdoc cref="ICardGenerator"/>
    public class InMemoryPlayingCardsGenerator : ICardGenerator
    {
        /// <summary>
        /// The number of cards the deck started with.
        /// </summary>
        private readonly int numberOfCards;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryPlayingCardsGenerator"/> class.
        /// </summary>
        /// <param name="numberOfCards">The starting number of cards.</param>
        public InMemoryPlayingCardsGenerator(int numberOfCards = 52)
        {
            this.numberOfCards = numberOfCards;
        }
        
        /// <inheritdoc/>
        /// <remarks>
        /// TODO: this should call some kind of static collection of cards to be more efficient.
        /// </remarks>
        public Card[] GetCardArray()
        {
            var values = new[]
            {
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9",
                "10",
                "J",
                "Q",
                "K",
                "A"
            };
            var suits = new ISuit[]
            {
                new Suit("Hearts", "Red"),
                new Suit("Diamonds", "Red"),
                new Suit("Spades", "Black"),
                new Suit("Clubs", "Black")
            };

            return suits.SelectMany(s => values.Select(v => new Card(v, s))).Take(this.numberOfCards).ToArray();
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<Card> GetCards()
        {
            return this.Convert(this.GetCardArray());
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<Card> Convert(IEnumerable<Card> cards)
        {
            return new Stack<Card>(cards);
        }
    }
}