using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace Labyrinth.Maps
{
    public class OutOfBounds : Exception
    {
        public Point Position { get; }

        public OutOfBounds(Point position)
        {
            Position = position;
        }

        public OutOfBounds(Point position, string message)
            : base(message)
        {
            Position = position;
        }

        public OutOfBounds(Point position, string message, Exception innerException)
            : base(message, innerException)
        {
            Position = position;
        }

        protected OutOfBounds(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}