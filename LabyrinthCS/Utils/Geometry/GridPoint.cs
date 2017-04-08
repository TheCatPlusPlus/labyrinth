namespace Labyrinth.Utils.Geometry
{
    public static class GridPoint
    {
        public static readonly Vector2I Invalid = new Vector2I(-1, -1);

        public static bool IsInvalidPoint(this Vector2I point)
        {
            return point == Invalid;
        }

        public static bool IsInvalidPoint(this Vector2F point)
        {
            return point == Invalid;
        }
    }
}
