namespace Appian.Domain
{
    /// <inheritdoc cref="ISuit"/>
    public class Suit : ISuit
    {
        /// <summary>
        /// The name of the suit, ie: "Hearts".
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The color of the suit, ie: "Red".
        /// </summary>
        private readonly string color;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Suit"/> class.
        /// </summary>
        /// <param name="name">The name of the suit.</param>
        /// <param name="color">The color of the suit.</param>
        public Suit(string name, string color)
        {
            this.name = name;
            this.color = color;
        }

        /// <inheritdoc/>
        public string GetName() => this.name;

        /// <inheritdoc/>
        public string GetColor() => this.color;
    }
}