using System.Collections.Generic;
using System.Drawing;
using System.Text;

using BearLib;

using JetBrains.Annotations;

using Labyrinth.Database;
using Labyrinth.Geometry;
using Labyrinth.Map;
using Labyrinth.Utils;

namespace Labyrinth.UI
{
	public sealed class LookAt
	{
		private static readonly Color Name = Color.Cyan;
		private static readonly Color Tile = Color.White;
		private static readonly Color Description = Color.LightGray;

		private readonly Rect _rect;
		private readonly StringBuilder _builder;

		public LookAt(Rect rect)
		{
			_rect = rect;
			_builder = new StringBuilder();
		}

		public void Draw([NotNull] Tile tile)
		{
			_builder.Clear();

			Add($"[color={Tile.ToHex()}]");

			if (!tile.EffectiveFlags.Contains(TileFlag.Seen))
			{
				Add("An unseen tile.");
			}
			else
			{
				ExamineTile(tile);
			}

			var text = _builder.ToString();
			Terminal.Print(_rect, text);
		}

		private void ExamineTile([NotNull] Tile tile)
		{
			var data = DB.Tiles.Get(tile.Type);
			var name = data.Name.Singular().Capitalize();
			var description = "";

			Add($"{name}.");

			if (tile.EffectiveFlags.Contains(TileFlag.Lit))
			{
				description = ExamineTileDetail(tile);
			}

			Add($"\n\n[color={Description.ToHex()}]{description}[/color]");
		}

		private string ExamineTileDetail([NotNull] Tile tile)
		{
			if (tile.Creature != null)
			{
				var data = DB.Entities.Get(tile.Creature.ID);
				var name = data.Name.Singular().Capitalize();
				AddSeparate($"[color={Name.ToHex()}]{name}[/color] is here.");
			}

			if (tile.Items.Count > 0)
			{
				ExamineTileItems(tile);
			}

			// TODO stats
			return "";
		}

		private void ExamineTileItems([NotNull] Tile tile)
		{
			if (tile.ItemCount.Count.Within(1, 5))
			{
				var items = new List<string>();

				foreach (var (id, count) in tile.ItemCount)
				{
					var data = DB.Entities.Get(id);
					var name = count > 1 ? data.Name.Plural(count) : data.Name.Singular();
					name = $"[color={Name.ToHex()}]{name}[/color]";
					items.Add(name);
				}

				items[0] = items[0].Capitalize();
				if (items.Count > 1)
				{
					items[items.Count - 1] = $"and {items[items.Count - 1]}";
				}

				var lies = items.Count > 1 ? "lie" : "lies";
				var joined = string.Join(", ", items);
				AddSeparate($"{joined} {lies} here.");
			}
			else
			{
				AddSeparate("Many items lie here.");
			}
		}

		private void Add(string text)
		{
			_builder.Append(text);
		}

		private void AddSeparate(string text)
		{
			if (_builder[_builder.Length - 1] != ' ')
			{
				_builder.Append(' ');
			}

			Add(text);
		}
	}
}
