﻿using System.Collections.Generic;

using Labyrinth.Utils.Geometry;

namespace Labyrinth.AI
{
    public interface IPathFinder
    {
        bool Found { get; }
        IEnumerable<Vector2I> Points { get; }
        Vector2I Start { get; }
        Vector2I Goal { get; }
    }
}