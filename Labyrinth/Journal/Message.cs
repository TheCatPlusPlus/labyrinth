using System;

using JetBrains.Annotations;

namespace Labyrinth.Journal
{
	public struct Message : IEquatable<Message>
	{
		public ulong ID { get; }
		public ulong Round { get; }
		public string Text { get; }
		public MessageType Type { get; }

		public Message(ulong id, ulong round, string text, MessageType type)
		{
			ID = id;
			Round = round;
			Text = text;
			Type = type;
		}

		public bool Equals(Message other)
		{
			return ID == other.ID;
		}

		public override bool Equals([CanBeNull] object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			return obj is Message message && Equals(message);
		}

		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}

		public static bool operator==(Message left, Message right)
		{
			return left.Equals(right);
		}

		public static bool operator!=(Message left, Message right)
		{
			return !left.Equals(right);
		}
	}
}
