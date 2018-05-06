namespace Labyrinth.Database
{
	public static class DB
	{
		public static readonly EntityID CreaturePlayer = new EntityID($"{EntityID.Creatures}/Player");
		public static readonly EntityID CreatureRat = new EntityID($"{EntityID.Mobs}/Rat");

		public static EntityDB Entities { get; } = new EntityDB();
		public static TileDB Tiles { get; } = new TileDB();
	}
}
