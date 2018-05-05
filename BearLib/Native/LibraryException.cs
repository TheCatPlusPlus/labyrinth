using System;
using System.Runtime.Serialization;

namespace BearLib.Native
{
	[Serializable]
	public sealed class LibraryException : Exception
	{
		public LibraryException()
		{
		}

		public LibraryException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public LibraryException(string message)
			: base(message)
		{
		}

		public LibraryException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
