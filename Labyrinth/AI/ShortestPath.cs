using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using C5;

using JetBrains.Annotations;

using Labyrinth.Maps;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.AI
{
    public sealed class ShortestPath : IPathFinder
    {
        private sealed class Node
        {
            private readonly ShortestPath _parent;

            public Vector2I Point { get; }
            public int Score => _parent.FScore(Point);

            public Node(ShortestPath parent, Vector2I point)
            {
                _parent = parent;
                Point = point;
            }

            private bool Equals([NotNull] Node other)
            {
                return Equals(other.Point);
            }

            private bool Equals(Vector2I other)
            {
                return Point == other;
            }

            public override bool Equals([CanBeNull] object obj)
            {
                var other = obj as Node;

                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                return ReferenceEquals(this, other) || Equals(other);
            }

            public override int GetHashCode()
            {
                return Point.GetHashCode();
            }

            public static bool operator==([CanBeNull] Node left, [CanBeNull] Node right)
            {
                return Equals(left, right);
            }

            public static bool operator!=([CanBeNull] Node left, [CanBeNull] Node right)
            {
                return !(left == right);
            }

            public override string ToString()
            {
                return $"Node: {Point}";
            }
        }

        private sealed class NodeComparer : IComparer<Node>
        {
            public int Compare([CanBeNull] Node x, [CanBeNull] Node y)
            {
                Debug.Assert(x != null);
                Debug.Assert(y != null);

                return y.Score - x.Score;
            }
        }

        private readonly int[,] _fScore;
        private readonly int[,] _gScore;
        private readonly Vector2I[,] _cameFrom;
        private readonly List<Vector2I> _path;

        public bool Found => _path.Count > 0;
        public IReadOnlyList<Vector2I> Points => _path;
        public Vector2I Start { get; }
        public Vector2I Goal { get; }

        public ShortestPath([NotNull] Level level, Vector2I start, Vector2I goal)
        {
            Start = start;
            Goal = goal;

            var closed = new System.Collections.Generic.HashSet<Node>();
            var open = new IntervalHeap<Node>(new NodeComparer());

            _cameFrom = new Vector2I[level.Rect.Width, level.Rect.Height];
            _gScore = new int[level.Rect.Width, level.Rect.Height];
            _fScore = new int[level.Rect.Width, level.Rect.Height];
            _path = new List<Vector2I>();

            foreach (var point in level.Rect.Points)
            {
                CameFrom(point) = GridPoint.Invalid;
                GScore(point) = int.MaxValue;
                FScore(point) = int.MaxValue;
            }

            GScore(start) = 0;
            FScore(start) = Estimate(level, start, goal);

            open.Add(MakeNode(start));
            while (open.Count > 0)
            {
                var current = open.FindMin();
                open.DeleteMin();

                if (closed.Contains(current))
                {
                    continue;
                }

                closed.Add(current);

                if (current.Point == goal)
                {
                    Reconstruct(goal);
                    return;
                }

                foreach (var point in level[current.Point].Neighbours)
                {
                    var neighbour = MakeNode(point);
                    var tile = level[point];

                    if (closed.Contains(neighbour) || (!tile.CanWalkThrough && !tile.IsDoor))
                    {
                        continue;
                    }

                    var distance = Distance(current.Point, neighbour.Point);
                    var gScore = AddScores(GScore(current.Point), distance);

                    if (gScore >= GScore(neighbour.Point))
                    {
                        continue;
                    }

                    CameFrom(neighbour.Point) = current.Point;
                    GScore(neighbour.Point) = gScore;
                    FScore(neighbour.Point) = AddScores(gScore, Estimate(level, neighbour.Point, goal));

                    open.Add(neighbour);
                }
            }
        }

        [NotNull]
        private Node MakeNode(Vector2I point)
        {
            return new Node(this, point);
        }

        private ref int FScore(Vector2I point)
        {
            return ref _fScore[point.X, point.Y];
        }

        private ref int GScore(Vector2I point)
        {
            return ref _gScore[point.X, point.Y];
        }

        private ref Vector2I CameFrom(Vector2I point)
        {
            return ref _cameFrom[point.X, point.Y];
        }

        private void Reconstruct(Vector2I current)
        {
            do
            {
                _path.Add(current);
                current = CameFrom(current);
            }
            while (current != GridPoint.Invalid);
        }

        private static int Estimate([NotNull] Level level, Vector2I node, Vector2I goal)
        {
            var moveCost = level[node]
                .Neighbours
                .Select(p => level[p].BaseMoveCost)
                .Concat(new[] { int.MaxValue })
                .Min();

            // surrounded by impassable tiles
            if (moveCost == int.MaxValue)
            {
                return int.MaxValue;
            }

            return Distance(node, goal) * moveCost;
        }

        private static int Distance(Vector2I a, Vector2I b)
        {
            return (a - b).NormL1();
        }

        private static int AddScores(int a, int b)
        {
            if ((a == int.MaxValue) || (b == int.MaxValue))
            {
                return int.MaxValue;
            }

            return a + b;
        }

        public IEnumerator<Vector2I> GetEnumerator()
        {
            return _path.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
