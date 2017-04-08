using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using BearLib;

using JetBrains.Annotations;

using Labyrinth.UI.Input;
using Labyrinth.Utils;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.UI
{
    public sealed class MenuWidget : IEnumerable<MenuWidget.Item>
    {
        public class Item
        {
            private readonly Func<bool> _isEnabled;

            public string Label { get; }
            public Action Action { get; }
            public int Width { get; }
            public bool Enabled => _isEnabled();

            public Item(string label, Action action, Func<bool> isEnabled)
            {
                Label = label;
                Action = action;
                _isEnabled = isEnabled;
                Width = Terminal.Measure(label).Width;
            }
        }

        public enum Result
        {
            None,
            Confirm,
            Cancel
        }

        private static readonly string Cursor = "->";

        private readonly List<Item> _choices;
        private int _current;

        public int Width { get; private set; }
        public int Height => _choices.Count;
        public Item Current => _choices[_current];

        public MenuWidget()
        {
            _choices = new List<Item>();
        }

        public MenuWidget([NotNull] IEnumerable<Item> items)
        {
            _choices = new List<Item>(items);
        }

        public void Add(string label, Action action, Func<bool> isEnabled = null)
        {
            if (isEnabled == null)
            {
                isEnabled = () => true;
            }

            _choices.Add(new Item(label, action, isEnabled));
            Width = _choices.Max(item => item.Width + Cursor.Length + 1);
        }

        private void MoveCursor(int delta)
        {
            _current = MathExt.Mod(_current + delta, _choices.Count);
        }

        public Result React(Event @event)
        {
            if (!(@event is KeyEvent key))
            {
                return Result.None;
            }

            var action = KeyMap.Menu[key.Key];

            switch (action)
            {
                case UserAction.Confirm:
                    if (Current.Enabled)
                    {
                        return Result.Confirm;
                    }

                    break;
                case UserAction.Cancel:
                    return Result.Cancel;
                case UserAction.NextItem:
                    MoveCursor(+1);
                    break;
                case UserAction.PreviousItem:
                    MoveCursor(-1);
                    break;
            }

            return Result.None;
        }

        public void Draw(Vector2I point)
        {
            for (var index = 0; index < _choices.Count; index++)
            {
                var choice = _choices[index];
                var isCurrent = index == _current;

                Color color;
                if (!choice.Enabled)
                {
                    color = Color.DarkGray;
                }
                else if (isCurrent)
                {
                    color = Color.LawnGreen;
                }
                else
                {
                    color = Color.White;
                }

                var cursor = isCurrent ? Cursor : new string(' ', Cursor.Length);
                using (TerminalExt.Foreground(color))
                {
                    Terminal.Print(point.X, point.Y + index, $"{cursor} {choice.Label}");
                }
            }
        }

        public IEnumerator<Item> GetEnumerator()
        {
            return _choices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
