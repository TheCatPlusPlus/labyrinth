using System.Text;

using JetBrains.Annotations;

namespace Labyrinth.Utils
{
    public static class StringExt
    {
        [NotNull]
        public static string Capitalize([NotNull] this string value)
        {
            return value.Substring(0, 1).ToUpperInvariant() + value.Substring(1);
        }

        [NotNull]
        public static string Repeat(this char value, int count)
        {
            return new string(value, count);
        }

        [NotNull]
        public static string Repeat(this string value, int count)
        {
            var builder = new StringBuilder();

            while (count > 0)
            {
                builder.Append(value);
                --count;
            }

            return builder.ToString();
        }
    }
}