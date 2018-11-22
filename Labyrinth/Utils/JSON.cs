using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Labyrinth.Utils
{
	public sealed class JSON
	{
		public static readonly JsonSerializerSettings Settings;
		public static readonly JsonSerializer Serializer;
		public static readonly Encoding Encoding;

		static JSON()
		{
			Settings = new JsonSerializerSettings
			{
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
				Culture = CultureInfo.InvariantCulture,
				DateFormatHandling = DateFormatHandling.IsoDateFormat,
				DateParseHandling = DateParseHandling.DateTimeOffset,
				NullValueHandling = NullValueHandling.Include,
				TypeNameHandling = TypeNameHandling.Auto,
				FloatFormatHandling = FloatFormatHandling.String,
				FloatParseHandling = FloatParseHandling.Double,
				Formatting = Formatting.None,
				MissingMemberHandling = MissingMemberHandling.Ignore,
				DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
				DefaultValueHandling = DefaultValueHandling.Include | DefaultValueHandling.Populate,
				MetadataPropertyHandling = MetadataPropertyHandling.Default,
				ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
				TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
			};

			Settings.Converters.Add(new StringEnumConverter());

			Encoding = new UTF8Encoding(false);
			Serializer = JsonSerializer.CreateDefault(Settings);
		}

		public static T Load<T>(string path)
		{
			try
			{
				using (var stream = new StreamReader(path, Encoding))
				using (var json = new JsonTextReader(stream))
				{
					var result = Serializer.Deserialize<T>(json);

					if (typeof(T).IsClass && ReferenceEquals(result, null))
					{
						throw new JsonException($"No object found on {path}");
					}

					return result;
				}
			}
			catch (DirectoryNotFoundException e)
			{
				throw new FileNotFoundException(e.Message, e);
			}
			catch (IsolatedStorageException e)
			{
				throw new FileNotFoundException(e.Message, e);
			}
		}

		public static void Save<T>(string path, T value)
		{
			var tempPath = path + ".tmp";
			Directory.CreateDirectory(Path.GetDirectoryName(tempPath));

			// we write to temp file first to avoid leaving
			// partially written one in case something breaks
			// (though Windows doesn't have an atomic rename, so
			// there's still a potential issue if File.Replace
			// fails in the middle)
			using (var stream = new StreamWriter(tempPath, false, Encoding))
			using (var json = new JsonTextWriter(stream))
			{
				Serializer.Serialize(json, value);
			}

			File.Replace(tempPath, path, null);
		}

		public static T Clone<T>(T value)
		{
			var buffer = new MemoryStream();

			using (var writer = new StreamWriter(buffer, Encoding))
			using (var json = new JsonTextWriter(writer))
			{
				Serializer.Serialize(json, value);
			}

			buffer.Position = 0;

			using (var reader = new StreamReader(buffer, Encoding))
			using (var json = new JsonTextReader(reader))
			{
				return Serializer.Deserialize<T>(json);
			}
		}
	}
}
