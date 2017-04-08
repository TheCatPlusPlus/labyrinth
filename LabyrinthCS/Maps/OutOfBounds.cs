using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

using Labyrinth.Utils.Geometry;

namespace Labyrinth.Maps
{
    [Serializable]
    public sealed class OutOfBounds : Exception
    {
        public Vector2I Position { get; }

        public OutOfBounds(Vector2I position)
        {
            Position = position;
        }

        public OutOfBounds(Vector2I position, string message)
            : base(message)
        {
            Position = position;
        }

        public OutOfBounds(Vector2I position, string message, Exception innerException)
            : base(message, innerException)
        {
            Position = position;
        }

        private OutOfBounds([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
