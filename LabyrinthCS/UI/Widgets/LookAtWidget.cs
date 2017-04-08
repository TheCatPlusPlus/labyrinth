using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using BearLib;

using JetBrains.Annotations;

using Labyrinth.Data;
using Labyrinth.Maps;
using Labyrinth.Utils;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.UI.Widgets
{
    public sealed class LookAtWidget
    {
        private static readonly Color Name = Color.Cyan;
        private static readonly Color Tile = Color.White;
        private static readonly Color Description = Color.LightGray;

        private readonly Rect _rect;
        private readonly StringBuilder _builder;

        public LookAtWidget(Rect rect)
        {
            _rect = rect;
            _builder = new StringBuilder();
        }

        public void Draw([NotNull] Tile tile)
        {
            _builder.Clear();

            Add($"[color=#{Tile.ToHex()}]");
            if (!tile.WasSeen)
            {
                Add("An unseen tile.");
            }
            else
            {
                ExamineTile(tile);
            }

            Terminal.Print(_rect, _builder.ToString());
        }

        private void ExamineTile([NotNull] Tile tile)
        {
            Add(tile.Name.Singular().Capitalize());
            var description = tile.Description;

            if (tile.IsLit)
            {
                ExamineTileDetail(tile, ref description);
            }

            Add($"\n\n[color=#{Description.ToHex()}]{description}");
        }

        private void ExamineTileDetail([NotNull] Tile tile, ref string description)
        {
            if (tile.Monster != null)
            {
                var name = tile.Monster.Name.Singular().Capitalize();
                AddSeparate($"[color={Name.ToHex()}]{name}[/color] is here.");
            }

            if (tile.Items.Count > 0)
            {
                ExamineTileItems(tile);
            }

            if (tile.Monster != null)
            {
                description = tile.Monster.Description;
            }
            else if (tile.Items.Count == 1)
            {
                description = tile.Items[0].Description;
            }
        }

        private void ExamineTileItems([NotNull] Tile tile)
        {
            // TODO store count on Tile maybe
            var query = from @group in tile.Items.GroupBy(i => i.Id)
                let item = ItemData.For(@group.Key)
                let count = @group.Count()
                select (item, count);
            var grouped = query.ToList();

            if (grouped.Count.Within(1, 5))
            {
                var items = new List<string>();

                for (var idx = 0; idx < grouped.Count; ++idx)
                {
                    (var item, var count) = grouped[idx];
                    var name = count > 1 ? item.Name.Plural(count) : item.Name.Singular();

                    if (idx == 0)
                    {
                        name = name.Capitalize();
                    }

                    name = $"[color={Name.ToHex()}]{name}[/color]";

                    if ((grouped.Count > 1) && (idx == (grouped.Count - 1)))
                    {
                        name = $"and {name}";
                    }

                    items.Add(name);
                }

                var lies = tile.Items.Count > 1 ? "lie" : "lies";
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
