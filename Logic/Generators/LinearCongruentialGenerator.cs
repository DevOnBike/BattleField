namespace Logic.Generators
{
    public class LinearCongruentialGenerator
    {
        private long _seed;
        private const long _a = 1664525;
        private const long _c = 1013904223;
        private const long _m = 4294967296; // 2^32
        
        public static readonly LinearCongruentialGenerator Instance = new();

        public LinearCongruentialGenerator(long seed)
        {
            _seed = seed;
        }

        public LinearCongruentialGenerator() : this(DateTime.Now.Ticks)
        {
        }

        public int Next()
        {
            _seed = (_a * _seed + _c) % _m;
            return (int)_seed;
        }
    }
}