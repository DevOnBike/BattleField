namespace Logic.Generators
{
    public class MersenneTwister
    {
        // Constants for MT19937
        private const int N = 624;
        private const int M = 397;
        private const uint MATRIX_A = 0x9908B0DF; // Constant vector a
        private const uint UPPER_MASK = 0x80000000; // Most significant w-r bits
        private const uint LOWER_MASK = 0x7FFFFFFF; // Least significant r bits

        private uint[] mt = new uint[N]; // The array for the state vector
        private int mti = N + 1; // mti==N+1 means mt[N] is not initialized

        public static readonly MersenneTwister Instance = new((uint)DateTime.UtcNow.Ticks);

        // Initializes the generator with a seed
        public MersenneTwister(uint seed)
        {
            InitGenRand(seed);
        }

        // Initialize the array with a seed
        private void InitGenRand(uint seed)
        {
            mt[0] = seed;
            for (mti = 1; mti < N; mti++)
            {
                mt[mti] = (uint)(1812433253 * (mt[mti - 1] ^ (mt[mti - 1] >> 30)) + mti);
            }
        }

        // Generate a random number on [0, 0xffffffff]-interval
        public uint GenRandUInt32()
        {
            uint y;
            uint[] mag01 = { 0x0, MATRIX_A }; // mag01[x] = x * MATRIX_A

            if (mti >= N)
            {
                int kk;

                // If seed has not been initialized with a number
                if (mti == N + 1)
                {
                    InitGenRand(5489); // Default seed
                }

                for (kk = 0; kk < N - M; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1];
                }

                for (; kk < N - 1; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + (M - N)] ^ (y >> 1) ^ mag01[y & 0x1];
                }

                y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
                mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1];

                mti = 0;
            }

            y = mt[mti++];

            // Tempering
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9D2C5680;
            y ^= (y << 15) & 0xEFC60000;
            y ^= (y >> 18);

            return y;
        }

        // Generate a random number in [0, 1) (floating-point)
        public double GenRandDouble()
        {
            return GenRandUInt32() * (1.0 / 4294967296.0); // Divide by 2^32
        }

        // Generate a random number in [0, range)
        public int GenRandInt(int range)
        {
            return (int)(GenRandUInt32() % (uint)range);
        }
    }
}