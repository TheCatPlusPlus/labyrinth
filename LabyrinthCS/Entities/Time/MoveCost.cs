using Labyrinth.Utils;

namespace Labyrinth.Entities.Time
{
    public static class MoveCost
    {
        public static int ApplyFactor(float factor, int @base = Const.SpeedBase)
        {
            return MathExt.RoundInt(factor * @base);
        }
    }
}
