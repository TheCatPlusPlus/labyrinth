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

		public static TileType GetOpened(this TileType type)
		{
			switch (type)
			{
				case TileType.DoorClosed:
				case TileType.DoorOpen:
					return TileType.DoorOpen;
				default:
					return type;
			}
		}
	}
}
