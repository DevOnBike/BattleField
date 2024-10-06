namespace Logic.Generators
{
    public class XorshiftGenerator
    {
        private uint _state;

        public static readonly XorshiftGenerator Instance = new();

        public XorshiftGenerator(uint seed)
        {
            _state = seed;
        }

        public XorshiftGenerator() : this((uint)DateTime.Now.Ticks)
        {
        }

        public uint Next()
        {
            _state ^= _state << 13;
            _state ^= _state >> 17;
            _state ^= _state << 5;

            return _state;
        }

        public int NextInt32()
        {
            return (int)(Next() % int.MaxValue);
        }
    }
}