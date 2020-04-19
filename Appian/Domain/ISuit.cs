namespace Appian.Domain
{
    /// <summary>
    /// Defines the suit of a card. The suit defines a type of card group with features like
    /// a name and shared color. This should be extended to provide additional grouping properties.
    /// </summary>
    public interface ISuit
    {
        /// <summary>
        /// Gets the name of the suit, ie: "Hearts".
        /// </summary>
        /// <returns>The name of the suit.</returns>
        string GetName();

        /// <summary>
        /// Gets the color of the suit, ie: "Red".
        /// </summary>
        /// <returns>The color of the suit.</returns>
        string GetColor();
    }
}