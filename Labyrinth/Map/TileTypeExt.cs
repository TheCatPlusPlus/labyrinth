namespace Labyrinth.Map
{
	public static class TileTypeExt
	{
		public static bool IsOpaqueWall(this TileType type)
		{
			switch (type)
			{
				case TileType.DeepWall:
				case TileType.Wall:
					return true;
				default:
					return false;
			}
		}
	}
}
