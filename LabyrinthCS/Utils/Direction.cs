using System.Collections.Generic;
using System.Drawing;

using Labyrinth.UI.Input;

namespace Labyrinth.Utils
{
    public static class Direction
    {
        public static readonly Size North = new Size(0, -1);
        public static readonly Size South = new Size(0, +1);
        public static readonly Size West = new Size(-1, 0);
        public static readonly Size East = new Size(+1, 0);

        public static readonly Size NorthEast = new Size(+1, -1);
        public static readonly Size NorthWest = new Size(-1, -1);
        public static readonly Size SouthEast = new Size(+1, +1);
        public static readonly Size SouthWest = new Size(-1, +1);

        public static readonly Dictionary<UserAction, Size> Movement = new Dictionary<UserAction, Size>
        {
            { UserAction.MoveNorth, North },
            { UserAction.MoveSouth, South },
            { UserAction.MoveWest, West },
            { UserAction.MoveEast, East },
            { UserAction.MoveNorthEast, NorthEast },
            { UserAction.MoveNorthWest, NorthWest },
            { UserAction.MoveSouthEast, SouthEast },
            { UserAction.MoveSouthWest, SouthWest }
        };
    }
}
