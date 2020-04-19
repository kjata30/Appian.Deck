namespace Appian.Deck.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Appian.Domain;
    using Appian.Generator;

    /// <summary>
    /// A standard, 52-card deck of playing cards.
    /// </summary>\
    /// <inheritdoc cref="Deck"/>
    public class PlayingCards : Deck
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayingCards"/> class.
        /// </summary>
        /// <inheritdoc cref="Deck"/>
        public PlayingCards(ICardGenerator cardGenerator)
            : base(cardGenerator)
        {
        }

        /// <exception cref="InvalidOperationException">Thrown when the deck is out of cards and should be handled.</exception>
        /// <exception cred="InvalidCastException">Thrown when the deck hasn't been initialized correctly.</exception>
        /// <inheritdoc/>
        public override IEnumerable<Card> Deal(int amount)
        {
            if (!(this.Cards is Stack<Card> cardStack))
            {
                throw new InvalidCastException("The underlying data structure is invalid for this deck type.");
            }

            for (var i = 0; i < amount; i++)
            {
                yield return cardStack.Pop();
            }
        }

        /// <exception cref="InvalidOperationException">Thrown when the deck is out of cards and should be handled.</exception>
        /// <inheritdoc/>
        public override Card DealOneCard()
        {
            return this.Deal(1).FirstOrDefault();
        }
    }
}