using Appian.Domain;

namespace Appian.Deck
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Appian.ExtensionMethods;
    using Appian.Generator;

    /// <summary>
    /// Provides base functionality for various decks of cards. Sub classes must initialize the underlying card collection
    /// using a data structure suitable for the type dealing, shuffling, and other supported functions.
    /// </summary>
    public abstract class Deck : IDeck, IEnumerable<Card>
    {
        /// <summary>
        /// The generator used to create the collection of cards in the deck.
        /// </summary>
        private readonly ICardGenerator generator;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Deck"/> class. 
        /// </summary>
        /// <param name="generator">The generator used to create the collection of cards in the deck.</param>
        protected Deck(ICardGenerator generator)
        {
            this.generator = generator;
            this.Cards = generator.GetCards();
        }
        
        /// <summary>
        /// Gets the underlying collection of cards within the deck.
        /// </summary>
        protected IReadOnlyCollection<Card> Cards { get; private set; }
        
        /// <inheritdoc/>
        public IEnumerator<Card> GetEnumerator()
        {
            return this.Cards.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        /// <summary>
        /// A Fisher-Yates-like, in-place shuffle that randomizes the card order in the deck.
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle is the link.
        /// TODO: should another override accept an external shuffling class.
        /// </remarks>
        /// <inheritdoc/>
        public virtual void Shuffle()
        {
            var cardArr = this.Cards.ToArray();

            // see the remarks on CryptoRandom class to read on why this is used over Random here.
            var random = new CryptoRandom();
            for (var i = this.Cards.Count - 1; i >= 1; i--)
            {
                cardArr.Swap(i, random.Next(i + 1));
            }
            
            this.Replenish(cardArr);
        }

        /// <inheritdoc/>
        public abstract IEnumerable<Card> Deal(int amount);

        /// <inheritdoc/>
        public abstract Card DealOneCard();

        /// <summary>
        ///     Handle replenishing of deck from some collection of cards. By default, this replaces the cards currently
        ///     in the deck with those provided. Optionally retains cards still in the deck.
        /// </summary>
        /// <param name="cards">The cards to be added.</param>
        /// <param name="keepCurrentCards">A flag indicating whether the cards currently in the deck should be kept.</param>
        protected virtual void Replenish(Card[] cards = null, bool keepCurrentCards = false)
        {
            if (cards == null)
            {
                cards = this.generator.GetCardArray();
            }
            
            if (keepCurrentCards)
            {
                var current = this.Cards.ToArray();
                var combined = new Card[cards.Length + current.Length];

                // TODO: does order matter? should the new cards go on the top or bottom? should this be configurable?
                for (var i = 0; i < current.Length; i++)
                {
                    combined[i] = current[i];
                }

                for (var i = 0; i < cards.Length; i++)
                {
                    combined[i + current.Length] = cards[i];
                }

                this.Cards = this.generator.Convert(combined);
            }
            else
            {
                this.Cards = this.generator.Convert(cards);
            }
        }
    }
}