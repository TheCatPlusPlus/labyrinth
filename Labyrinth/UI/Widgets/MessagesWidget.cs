using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using BearLib;

using Labyrinth.Messages;
using Labyrinth.UI.Input;
using Labyrinth.Utils;
using Labyrinth.Utils.Geometry;

namespace Labyrinth.UI.Widgets
{
    public sealed class MessagesWidget : IMessageListener
    {
        private sealed class More : Modal<Null>
        {
            protected override void DrawModal()
            {
            }

            protected override void ReactModal(KeyEvent @event)
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (KeyMap.Menu[@event.Key])
                {
                    case UserAction.Confirm:
                    case UserAction.Cancel:
                        Close(null);
                        break;
                }
            }
        }

        private sealed class Entry
        {
            public string Message { get; }
            public int Repeated { get; set; }

            public Entry(string message)
            {
                Message = message;
                Repeated = 1;
            }

            public override string ToString()
            {
                var repeat = Repeated > 1 ? $" (x{Repeated})" : "";
                return $"{Message}{repeat}";
            }
        }

        private sealed class Chunk
        {
            public int Round { get; }
            public List<Entry> Entries { get; }
            public int Height { get; set; }

            public Chunk(int round)
            {
                Entries = new List<Entry>();
                Round = round;
            }

            public override string ToString()
            {
                return string.Join(" ", Entries);
            }
        }

        private readonly Rect _rect;
        private readonly List<Chunk> _chunks;
        private bool _more;

        public MessagesWidget(Rect rect)
        {
            _rect = rect;
            _chunks = new List<Chunk>();
        }

        public void Draw()
        {
            var y = _rect.Bottom - 2;
            Rect GetRect(int h) => new Rect(_rect.X, y, _rect.Width, h);

            var more = GetRect(1);

            // see ClearArea below
            using (TerminalExt.Layer(32))
            {
                if (_more)
                {
                    using (TerminalExt.Foreground(Color.Red))
                    {
                        Terminal.Print(more, ContentAlignment.MiddleCenter, "--more--");
                    }
                }

                // TODO annoying case: single chunk that doesn't fit in the log and needs
                // to be drawn in two or more parts, and each should pause

                var idx = _chunks.Count - 1;
                while ((y > _rect.Top) && (idx >= 0))
                {
                    var fg = idx == (_chunks.Count - 1) ? Color.White : Color.DarkGray.Darken(0.6f);
                    var chunk = _chunks[idx];
                    y -= chunk.Height;
                    idx--;

                    using (TerminalExt.Foreground(fg))
                    {
                        Terminal.Print(GetRect(chunk.Height), ContentAlignment.TopLeft, chunk.ToString());
                    }
                }

                // crop out the part that overflows (this is why a random high layer is used)
                // this wonderful hack brought to you by terminal_crop being worthless
                var clear = new Rect(_rect.X, 0, _rect.Width, _rect.Y);
                Terminal.ClearArea(clear);

                if (idx >= 0)
                {
                    // these are completely off screen
                    _chunks.RemoveRange(0, idx + 1);
                }
            }
        }

        public void OnMessage(string message, int round, bool important)
        {
            if ((_chunks.Count == 0) || (_chunks.Last().Round != round))
            {
                _chunks.Add(new Chunk(round));
            }

            var chunk = _chunks.Last();

            if (chunk.Entries.Count > 0)
            {
                var last = chunk.Entries.Last();
                if (last.Message == message)
                {
                    last.Repeated++;
                }
                else
                {
                    chunk.Entries.Add(new Entry(message));
                }
            }
            else
            {
                chunk.Entries.Add(new Entry(message));
            }

            chunk.Height = Terminal.Measure(new Vector2I(_rect.Width, 0), chunk.ToString()).Height;

            if (important)
            {
                _more = true;
                new More().Run();
                _more = false;
            }
        }
    }
}
