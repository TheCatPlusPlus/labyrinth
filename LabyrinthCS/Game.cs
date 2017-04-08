using JetBrains.Annotations;

using Labyrinth.Data;
using Labyrinth.Entities;
using Labyrinth.Maps;
using Labyrinth.UI.Input;
using Labyrinth.Utils.Geometry;

namespace Labyrinth
{
    public sealed class Game
    {
        public Player Player { get; }
        public Zone Zone { get; private set; }
        public Level Level { get; private set; }
        public int TotalCost { get; private set; }
        public int LastCost { get; private set; }

        public Game([NotNull] string playerName)
        {
            Player = new Player(playerName);
            Zone = new Zone(ZoneData.Test);
            Level = Zone.CreateLevel(0);

            Player.Spawn(Level);
            Player.FieldOfView.Update(Level);
        }

        public void Start()
        {
            Level.Scheduler.Advance();
        }

        public void React(UserAction action)
        {
            LastCost = 0;

            if (Direction.Movement.TryGetValue(action, out Vector2I direction))
            {
                LastCost = Player.Move(Player.Position + direction);
            }

            if (LastCost > 0)
            {
                TotalCost += LastCost;
            }

            Player.FieldOfView.Update(Level);
            Level.Scheduler.Advance();
        }
    }
}
