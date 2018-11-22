using System.Runtime.Serialization;

using Labyrinth.ECS;

namespace Labyrinth.Gameplay.Components
{
	// for creatures: can open doors etc
	[DataContract]
	public sealed class Intelligent : IEntityComponent
	{
	}
}
