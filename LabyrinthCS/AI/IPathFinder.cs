using System.Collections.Generic;
using System.Drawing;

namespace Labyrinth.AI
{
    public interface IPathFinder
    {
        bool Found { get; }
        IEnumerable<Point> Points { get; }
    }
}
