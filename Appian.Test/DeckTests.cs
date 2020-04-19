namespace Appian.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Accord.Statistics.Distributions.Univariate;
    using Appian.Deck.Implementation;
    using Appian.Domain;
    using Appian.Generator.Implementation;
    using NUnit.Framework;

    /// <summary>
    /// Contains tests for the methods on <see cref="Deck"/>.
    /// </summary>
    public class DeckTests
    {
        #region Positive Tests
        
        /// <summary>
        /// Tests whether the shuffle is random.
        /// </summary>
        /// <remarks>
        /// This obviously isn't completely sufficient to test the randomness of the shuffle, but analyzing the RNG of 
        /// something like this is a mathematically difficult problem. Hopefully this is a good start.
        /// </remarks>
        [Test]
        public void IsShuffleSufficientlyRandom()
        {
            for (int i = 4; i < 7; i++)
            {
                var factorial = Factorial(i);
                var results = RunABunchOfShuffles(i);
            
                // there should be i! buckets... any different and our shuffler has an implementation bug.
                Assert.AreEqual(factorial, results.Count);

                // now we need to test whether the shuffling is biased using a chi squared test
                var dist = new ChiSquareDistribution(factorial - 1);
                var criticalValue = dist.InverseDistributionFunction(.95);
                var sum = results.Sum(kvp => Math.Pow(kvp.Value - 2000d, 2) / 2000d);
                Assert.Less(sum, criticalValue);
            }
        }

        /// <summary>
        /// Tests whether the deck runs out of cards after the last draw.
        /// </summary>
        [Test]
        public void DoesDeckRunOutOfCards()
        {
            var generator = new InMemoryPlayingCardsGenerator();
            var deck = new PlayingCards(generator);

            var cards = new List<Card>();
            for (var i = 0; i < 52; i++)
            {
                cards.Add(deck.DealOneCard());
            }

            Assert.AreEqual(cards.Count, 52);
            Assert.Throws<InvalidOperationException>(() => deck.DealOneCard());
        }

        /// <summary>
        /// Tests whether the deck actually deals the top card.
        /// </summary>
        [Test]
        public void DoesDeckAlwaysDealTopCard()
        {
            var generator = new InMemoryPlayingCardsGenerator();
            for (var i = 0; i < 1000; i++)
            {
                var deck = new PlayingCards(generator);
                deck.Shuffle();
                
                // Stack{T} pops the first element of the underlying array, which is reversed when the stack is constructed.
                var card = deck.ToArray().First();
                Assert.AreEqual(card, deck.DealOneCard());
            }
        }

        /// <summary>
        /// Tests whether the deck has the correct cards.
        /// </summary>
        [Test]
        public void DoesDeckHaveTheCorrectCards()
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

            var cards = suits.SelectMany(s => values.Select(v => new Card(v, s))).ToArray();
            var generator = new InMemoryPlayingCardsGenerator();
            var deck = new PlayingCards(generator);
            
            Assert.AreEqual(deck.Count(), 52);

            foreach (var card in deck.ToArray())
            {
                Assert.IsTrue(cards.Any(c => c.Value == card.Value && c.Suit.GetName() == card.Suit.GetName()));
            }
        }

        /// <summary>
        /// Tests that the entropy of the observations is within tolerance.
        /// </summary>
        /// <remarks>
        /// // https://math.stackexchange.com/questions/2916887/shannon-entropy-of-a-fair-dice is the link.
        /// </remarks>
        [Test]
        public void IsShufflerEntropyWithinTolerableLimits()
        {
            for (var i = 4; i < 7; i++)
            {
                var factorial = Factorial(i);
                var results = RunABunchOfShuffles(i);
                var entropy = -results.Sum(kvp => Math.Log2(kvp.Value / (factorial * 2000d)) * (kvp.Value / (factorial * 2000d)));
                var expected = -Math.Log2(1d / factorial);
                
                Assert.AreEqual(entropy, expected, .01);
            }
        }

        /// <summary>
        /// Computes the factorial of x.
        /// </summary>
        /// <param name="x">The number used to compute the factorial.</param>
        /// <returns>The factorial of x.</returns>
        /// <exception cref="ArgumentException">Thrown when the input is less than 1 or greater than 20.</exception>
        private static int Factorial(int x)
        {
            if (x > 20 || x < 1)
            {
                throw new ArgumentException("Calculation of integers greater than 20 or less than 1 not supported.", nameof(x));
            }

            int result = 1;
            for (int i = 1; i <= x; i++)
            {
                result *= i;
            }

            return result;
        }

        /// <summary>
        /// Runs deckSize! * 2000 deck shuffles and returns a dictionary of results.
        /// </summary>
        /// <param name="deckSize">The size of the deck to create and shuffle.</param>
        /// <returns>The results of the shuffling experiment.</returns>
        /// <exception cref="NotSupportedException">Thrown when the deck size is greater than 8. The <see cref="InMemoryPlayingCardsGenerator"/>
        /// and <see cref="PlayingCards"/> types support larger decks, but because the experiment grows with the factorial
        /// of the deck size, we have to be really careful about how large we allow it to grow.</exception>
        private static Dictionary<string, int> RunABunchOfShuffles(int deckSize)
        {
            if (deckSize > 8)
            {
                throw new NotSupportedException("That's too many cards to test this way.");
            }
            
            var generator = new InMemoryPlayingCardsGenerator(deckSize);
            var deck = new PlayingCards(generator);
            deck.Shuffle();
            var results = new Dictionary<string, int>();

            var factorial = Factorial(deckSize);
            
            for (var i = 0; i < factorial * 2000; i++)
            {
                var cards = deck.Deal(deckSize);
                var key = string.Join(" ", cards.Select(c => $"{c.Value}{c.Suit.GetName()}"));
                if (results.ContainsKey(key))
                {
                    results[key]++;
                }
                else
                {
                    results.Add(key, 1);
                }
                
                deck = new PlayingCards(generator);
                deck.Shuffle();
            }

            return results;
        }
        
        #endregion
        
        /*
         * Many other tests should really be run, including tests for initialization/method consumption edge cases
         * (zero cards in the deck, int.Maximum cards in the deck, etc). In a commercial product we would spend
         * considerably more time defining the use cases and creating tests to minimize risk of failure and increase
         * quality of our code, but that is a very time consuming process.
         */
    }
}