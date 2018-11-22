namespace Labyrinth.Utils
{
	public static class PathExt
	{
		public static string MakeRelative(string path, string root)
		{
			// https://stackoverflow.com/questions/703281/getting-path-relative-to-the-current-working-directory/19453551#19453551
			var result = string.Empty;
			int offset;

			// this is the easy case.  The file is inside of the working directory.
			if (path.StartsWith(root))
			{
				return path.Substring(root.Length + 1);
			}

			// the hard case has to back out of the working directory
			var baseDirs = root.Split(':', '\\', '/');
			var fileDirs = path.Split(':', '\\', '/');

			// if we failed to split (empty strings?) or the drive letter does not match
			if ((baseDirs.Length <= 0) || (fileDirs.Length <= 0) || (baseDirs[0] != fileDirs[0]))
			{
				// can't create a relative path between separate harddrives/partitions.
				return path;
			}

			// skip all leading directories that match
			for (offset = 1; offset < baseDirs.Length; offset++)
			{
				if (baseDirs[offset] != fileDirs[offset])
				{
					break;
				}
			}

			// back out of the working directory
			for (var i = 0; i < baseDirs.Length - offset; i++)
			{
				result += "..\\";
			}

			// step into the file path
			for (var i = offset; i < fileDirs.Length - 1; i++)
			{
				result += fileDirs[i] + "\\";
			}

			// append the file
			result += fileDirs[fileDirs.Length - 1];

			return result;
		}
	}
}
