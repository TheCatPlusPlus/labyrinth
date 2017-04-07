namespace Labyrinth.Utils
{
    public static class MiscExt
    {
        public static void Swap<T>(ref T left, ref T right)
        {
            var temp = left;
            left = right;
            right = temp;
        }
    }
}
