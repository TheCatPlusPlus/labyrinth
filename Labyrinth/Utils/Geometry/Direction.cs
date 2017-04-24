using System.Collections.Generic;

using Labyrinth.UI.Input;

namespace Labyrinth.Utils.Geometry
{
    public static class Direction
    {
        public static readonly Dictionary<UserAction, Vector2I> Movement = new Dictionary<UserAction, Vector2I>
        {
            { UserAction.MoveNorth, Vector2I.North },
            { UserAction.MoveSouth, Vector2I.South },
            { UserAction.MoveWest, Vector2I.West },
            { UserAction.MoveEast, Vector2I.East },
            { UserAction.MoveNorthEast, Vector2I.NorthEast },
            { UserAction.MoveNorthWest, Vector2I.NorthWest },
            { UserAction.MoveSouthEast, Vector2I.SouthEast },
            { UserAction.MoveSouthWest, Vector2I.SouthWest }
        };
    }
}
