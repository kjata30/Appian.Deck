namespace Appian.Domain
{
    /// <summary>
    /// Models a simple card.
    /// </summary>
    /// <remarks>
    /// It might also make sense to create an interface for this with explicit getters/setters for the value and suit,
    /// as well as support for other properties of the card.
    /// </remarks>
    public class Card
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        /// <param name="value">The value of the card (ie: "Ace").</param>
        /// <param name="suit">The <see cref="ISuit"/> of the card (ie: "Hearts").</param>
        public Card(string value, ISuit suit)
        {
            this.Value = value;
            this.Suit = suit;
        }
        
        /// <summary>
        /// Gets the value of the card.
        /// </summary>
        public string Value { get; }
        
        /// <summary>
        /// Gets the suit of the card.
        /// </summary>
        public ISuit Suit { get; }
    }
}