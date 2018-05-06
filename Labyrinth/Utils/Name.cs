using JetBrains.Annotations;

namespace Labyrinth.Utils
{
	public struct Name
	{
		private readonly string _singular;
		private readonly string _plural;
		private readonly string _article;
		private readonly bool _isCountable;
		private readonly bool _isProper;
		private readonly bool _isThing;
		private readonly bool _isUnique;

		public Name(
			string singular,
			string plural = "",
			string article = "",
			bool countable = true,
			bool unique = false,
			bool proper = false,
			bool thing = false)
		{
			if (string.IsNullOrEmpty(plural))
			{
				plural = singular + "s";
			}

			_singular = singular;
			_plural = plural;
			_article = string.IsNullOrEmpty(article) ? GetDefaultArticle(singular) : article;
			_isCountable = countable && !unique;
			_isUnique = unique;
			_isProper = proper;
			_isThing = thing;
		}

		[NotNull]
		public string Singular(bool definite = false)
		{
			if (_isProper)
			{
				return _singular;
			}

			if (!definite && !_isUnique)
			{
				return _isCountable
					? $"{_article} {_singular}"
					: _singular;
			}

			return $"the {_singular}";
		}

		[NotNull]
		public string Plural(int? count = null, bool article = false, bool definite = false)
		{
			if ((!_isCountable || (count == null)) && article)
			{
				return definite ? $"the {_plural}" : $"some {_plural}";
			}

			return count == null ? _plural : $"{count.Value} {_plural}";
		}

		[NotNull]
		public string Unseen()
		{
			return _isThing ? "something" : "someone";
		}

		[NotNull]
		private static string GetDefaultArticle(string singular)
		{
			switch (singular[0])
			{
				case 'e':
				case 'u':
				case 'i':
				case 'o':
				case 'a':
					return "an";
				default:
					return "a";
			}
		}

		[NotNull]
		public override string ToString()
		{
			return Singular(true);
		}
	}
}
