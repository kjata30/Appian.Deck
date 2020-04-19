namespace Appian
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    ///     A cryptographically strong random number generator.
    /// </summary>
    /// <remarks>
    ///     Required because we're dealing with publicly-facing calls that rely on randomness; pseudo-randomness can be
    ///     exploited.
    ///     This is essentially just lifted from Stephen Toub and Shawn Farkas' work.
    ///     https://docs.microsoft.com/en-us/archive/msdn-magazine/2007/september/net-matters-tales-from-the-cryptorandom is
    ///     the link.
    /// </remarks>
    /// <inheritdoc cref="Random" />
    public sealed class CryptoRandom : Random
    {
        private readonly RNGCryptoServiceProvider _rng =
            new RNGCryptoServiceProvider();

        private readonly byte[] _uint32Buffer = new byte[4];

        public CryptoRandom()
        {
        }

        public CryptoRandom(int ignoredSeed)
        {
        }

        public override int Next()
        {
            this._rng.GetBytes(this._uint32Buffer);
            return BitConverter.ToInt32(this._uint32Buffer, 0) & 0x7FFFFFFF;
        }

        public override int Next(int maxValue)
        {
            if (maxValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue));
            }

            return this.Next(0, maxValue);
        }

        public override int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue));
            }

            if (minValue == maxValue)
            {
                return minValue;
            }

            long diff = maxValue - minValue;
            while (true)
            {
                this._rng.GetBytes(this._uint32Buffer);
                var rand = BitConverter.ToUInt32(this._uint32Buffer, 0);

                var max = 1 + (long)uint.MaxValue;
                var remainder = max % diff;
                if (rand < max - remainder)
                {
                    return (int)(minValue + (rand % diff));
                }
            }
        }

        public override double NextDouble()
        {
            this._rng.GetBytes(this._uint32Buffer);
            var rand = BitConverter.ToUInt32(this._uint32Buffer, 0);
            return rand / (1.0 + uint.MaxValue);
        }

        public override void NextBytes(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            this._rng.GetBytes(buffer);
        }
    }
}