namespace Logic.Generators
{
    public class PermutedCongruentialGenerator
    {
        private ulong _state;
        private ulong _increment;
        
        public static readonly PermutedCongruentialGenerator Instance = new((ulong)DateTime.Now.Ticks, 1);

        public PermutedCongruentialGenerator(ulong seed, ulong increment)
        {
            _state = seed;
            _increment = increment;
        }

        private ulong Next()
        {
            var oldstate = _state;
            
            _state = oldstate * 6364136223846793005ul + (_increment | 1);
            
            var xorShifted = (uint)(((oldstate >> 18) ^ oldstate) >> 27);
            var rot = (uint)(oldstate >> 59);
            
            return (xorShifted >> (int)rot) | (xorShifted << (int)((-rot) & 31));
        }

        public uint NextUInt()
        {
            return (uint)Next();
        }
    }
}