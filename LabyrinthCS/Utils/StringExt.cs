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
    }
}