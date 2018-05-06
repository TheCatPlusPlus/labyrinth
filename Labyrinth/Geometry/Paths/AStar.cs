using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using C5;

using JetBrains.Annotations;

using Labyrinth.Database;
using Labyrinth.Entities;
using Labyrinth.Map;

namespace Labyrinth.Geometry.Paths
{
	// A* pathfinding for creatures
	public sealed class AStar
	{
		private sealed class Node : IEquatable<Node>
		{
			private readonly AStar _parent;

			public Int2 Point { get; }
			public int Score => _parent.FScore(Point);

			public Node(AStar parent, Int2 point)
			{
				_parent = parent;
				Point = point;
			}

			public bool Equals([CanBeNull] Node other)
			{
				if (ReferenceEquals(null, other))
				{
					return false;
				}

				if (ReferenceEquals(this, other))
				{
					return true;
				}

				return Point.Equals(other.Point);
			}

			public override bool Equals([CanBeNull] object obj)
			{
				if (ReferenceEquals(null, obj))
				{
					return false;
				}

				if (ReferenceEquals(this, obj))
				{
					return true;
				}

				return obj is Node node && Equals(node);
			}

			public override int GetHashCode()
			{
				return Point.GetHashCode();
			}

			public static bool operator==(Node left, Node right)
			{
				return Equals(left, right);
			}

			public static bool operator!=(Node left, Node right)
			{
				return !Equals(left, right);
			}

			[NotNull]
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
		private readonly Int2?[,] _cameFrom;
		private readonly List<Int2> _path;

		public static Path? Find(Creature creature, Int2 target)
		{
			Debug.Assert(creature.Level != null, "creature.Level != null");
			Debug.Assert(creature.Position != null, "creature.Position != null");

			return Find(creature.Level, creature.Position.Value, target, creature.CanGetThrough);
		}

		public static Path? Find(Level level, Int2 start, Int2 goal, Func<Tile, bool> canGetThrough)
		{
			var aStar = new AStar(level, start, goal, canGetThrough);

			if (aStar._path.Count == 0)
			{
				return null;
			}

			return new Path(aStar._path, start, goal);
		}

		private AStar(Level level, Int2 start, Int2 goal, Func<Tile, bool> canGetThrough)
		{
			var closed = new System.Collections.Generic.HashSet<Node>();
			var open = new IntervalHeap<Node>(new NodeComparer());

			_cameFrom = new Int2?[level.Width, level.Height];
			_gScore = new int[level.Width, level.Height];
			_fScore = new int[level.Width, level.Height];
			_path = new List<Int2>();

			foreach (var point in level.Grid.Rect.Points)
			{
				CameFrom(point) = null;
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

				var thisTile = level.Grid[current.Point];
				Debug.Assert(thisTile != null, "thisTile != null");

				foreach (var tile in thisTile.Neighbours)
				{
					var neighbour = MakeNode(tile.Position);

					if (closed.Contains(neighbour) || !canGetThrough(tile))
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
		private Node MakeNode(Int2 point)
		{
			return new Node(this, point);
		}

		private ref int FScore(Int2 point)
		{
			return ref _fScore[point.X, point.Y];
		}

		private ref int GScore(Int2 point)
		{
			return ref _gScore[point.X, point.Y];
		}

		private ref Int2? CameFrom(Int2 point)
		{
			return ref _cameFrom[point.X, point.Y];
		}

		private void Reconstruct(Int2 goal)
		{
			Int2? current = goal;
			while (current != null)
			{
				_path.Add(current.Value);
				current = CameFrom(current.Value);
			}
		}

		private static int Estimate([NotNull] Level level, Int2 node, Int2 goal)
		{
			var tile = level.Grid[node];

			// out of bounds
			if (tile == null)
			{
				return int.MaxValue;
			}

			var moveCost = tile.Neighbours
				.Select(t => DB.Tiles.Get(t.Type).CostMultiplier)
				.Concat(new[] { int.MaxValue })
				.Min();

			// surrounded by impassable tiles
			if (moveCost == int.MaxValue)
			{
				return int.MaxValue;
			}

			return Distance(node, goal) * moveCost;
		}

		private static int Distance(Int2 a, Int2 b)
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
	}
}
