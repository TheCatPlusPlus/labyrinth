using System.Collections.Generic;

using JetBrains.Annotations;

using NodaTime;

namespace Labyrinth.Messages
{
    public sealed class MessageLog : IMessageListener
    {
        public sealed class Entry
        {
            public ZonedDateTime Timestamp { get; }
            public string Message { get; }
            public bool Important { get; }
            public int Round { get; }

            public Entry(string message, int round, bool important)
            {
                Timestamp = SystemClock.Instance.GetCurrentInstant().InUtc();
                Message = message;
                Important = important;
                Round = round;
            }
        }

        private readonly List<Entry> _messages;

        [NotNull]
        [ItemNotNull]
        public IReadOnlyList<Entry> Messages => _messages;

        public MessageLog()
        {
            _messages = new List<Entry>();
        }

        public void OnMessage([NotNull] string message, int round, bool important)
        {
            var entry = new Entry(message, round, important);
            _messages.Add(entry);
        }
    }
}
