using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

using C5;

using JetBrains.Annotations;

using Labyrinth.Maps;
using Labyrinth.Utils;

namespace Labyrinth.AI
{
    public class ShortestPath : IPathFinder
    {
        private class Node
        {
            private readonly ShortestPath _parent;

            public Point Point { get; }
            public int Score => _parent.FScore(Point);

            public Node(ShortestPath parent, Point point)
            {
                _parent = parent;
                Point = point;
            }

            private bool Equals([NotNull] Node other)
            {
                return Equals(other.Point);
            }

            private bool Equals(Point other)
            {
                return Point == other;
            }

            public override bool Equals(object obj)
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

        private class NodeComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                Debug.Assert(x != null);
                Debug.Assert(y != null);

                return y.Score - x.Score;
            }
        }

        private readonly int[,] _fScore;
        private readonly int[,] _gScore;
        private readonly Point[,] _cameFrom;
        private readonly List<Point> _path;

        public bool Found => _path.Count > 0;
        public IEnumerable<Point> Points => _path;

        public ShortestPath([NotNull] Level level, Point start, Point goal)
        {
            var closed = new System.Collections.Generic.HashSet<Node>();
            var open = new IntervalHeap<Node>(new NodeComparer());

            _cameFrom = new Point[level.Rect.Width, level.Rect.Height];
            _gScore = new int[level.Rect.Width, level.Rect.Height];
            _fScore = new int[level.Rect.Width, level.Rect.Height];
            _path = new List<Point>();

            foreach (var point in level.Rect.Points())
            {
                CameFrom(point) = PointExt.Invalid;
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

                    if (closed.Contains(neighbour) || (!tile.IsWalkable && !tile.IsDoor))
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
        private Node MakeNode(Point point)
        {
            return new Node(this, point);
        }

        private ref int FScore(Point point)
        {
            return ref _fScore[point.X, point.Y];
        }

        private ref int GScore(Point point)
        {
            return ref _gScore[point.X, point.Y];
        }

        private ref Point CameFrom(Point point)
        {
            return ref _cameFrom[point.X, point.Y];
        }

        private void Reconstruct(Point current)
        {
            do
            {
                _path.Add(current);
                current = CameFrom(current);
            }
            while (current != PointExt.Invalid);
        }

        private static int Estimate([NotNull] Level level, Point node, Point goal)
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

        private static int Distance(Point a, Point b)
        {
            var x = Math.Abs(a.X - b.X);
            var y = Math.Abs(b.Y - a.Y);
            return x + y;
        }

        private static int AddScores(int a, int b)
        {
            if (a == int.MaxValue || b == int.MaxValue)
            {
                return int.MaxValue;
            }

            return a + b;
        }
    }
}
