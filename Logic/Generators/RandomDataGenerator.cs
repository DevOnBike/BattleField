using System.Collections.Concurrent;
using System.Text;

namespace Logic.Generators
{
    public static class RandomDataGenerator
    {
        private static readonly Random _randomizer = new(GetSeed());
        
        private static readonly ParallelOptions _parallelOptions = new()
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ ";

        public static int GetSeed()
        {
            unchecked
            {
                return (int)DateTime.Now.Ticks;
            }
        }

        public static string GetString(int length, string chars)
        {
            var sb = new StringBuilder();

            while (length-- > 0)
            {
                sb.Append(chars[_randomizer.Next(chars.Length)]);
            }
            return sb.ToString();
        }

        public static char GetChar(char[] chars)
        {
            return chars[_randomizer.Next(chars.Length)];
        }

        public static string GetStringParallel(int length, char[] chars)
        {
            var charSpan = new char[length];

            Parallel.ForEach(Partitioner.Create(0, length), _parallelOptions, (range, state) =>
            {
                for (var i = range.Item1; i < range.Item2; i++)
                {
                    charSpan[i] = GetChar(chars);                 
                }
            });
            
            return new string(charSpan);
        }
        
        
        public static string GetString(int length, char[] chars)
        {
            return string.Create(length, (length, chars, _randomizer), static (span, state) =>
            {
                var randomizer = state._randomizer;
                var cs = state.chars;
                var charsLength = cs.Length;
                
                for (var i = 0; i < state.length; i++)
                {
                    span[i] = cs[randomizer.Next(charsLength)];
                }
            });
            
        }
        
        public static string GetStringSpan(int length, char[] chars)
        {
            Span<char> charSpan = stackalloc char[length];
            
            _randomizer.GetItems(chars, charSpan);
            
            return charSpan.ToString();
        }

        public static string GetString()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GetString(int length)
        {
            return GetString(length, Chars);
        }

        public static string GetVariableName(int length = 10)
        {
            const string letters = "abcdefghijklmnopqrstuvwxyz";
            const string allowedChars = letters + "_1234567890";

            var sb = new StringBuilder();

            sb.Append(letters[_randomizer.Next(letters.Length)]);

            while (length-- > 0)
            {
                sb.Append(allowedChars[_randomizer.Next(allowedChars.Length)]);
            }
            return sb.ToString();
        }

        public static bool GetBoolean()
        {
            return _randomizer.Next(0, 2) == 0;
        }

        public static double Next()
        {
            return _randomizer.NextDouble();
        }

        public static void Fill(byte[] bytes)
        {
            _randomizer.NextBytes(bytes);
        }

        public static double GetInRange(double min, double max)
        {
            var d = _randomizer.NextDouble();
            var range = Math.Abs(max - min) + 1;
            return min + d * range;
        }

        public static int GetInRange(int min, int max)
        {
            return _randomizer.Next(min, max + 1);
        }

        public static DateTime GetDateTime(DateTime min, DateTime max)
        {
            var delta = max - min;
            var ticks = _randomizer.NextDouble() * delta.Ticks;
            return min.Add(TimeSpan.FromTicks((long)ticks));
        }

        public static T GetRandomEnum<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new NotSupportedException("Must be enum type");
            }

            var values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            var idx = GetInRange(0, values.Length - 1);
            return values[idx];
        }

    }
}
