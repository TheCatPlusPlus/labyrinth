using System.Collections.Generic;

using JetBrains.Annotations;

using Labyrinth.Data;
using Labyrinth.Entities;
using Labyrinth.Maps;
using Labyrinth.Messages;
using Labyrinth.UI.Input;
using Labyrinth.Utils.Geometry;

namespace Labyrinth
{
    public sealed class Game
    {
        public bool Started { get; private set; }
        public Player Player { get; }
        public MessageLog MessageLog { get; }
        public List<IMessageListener> MessageListeners { get; }

        public Zone Zone { get; private set; }
        public Level Level { get; private set; }
        public int TotalCost { get; private set; }
        public int LastCost { get; private set; }
        public int Round { get; private set; }

        public Game([NotNull] string playerName)
        {
            MessageLog = new MessageLog();
            MessageListeners = new List<IMessageListener> { MessageLog };

            Player = new Player(playerName);
            Zone = new Zone(ZoneData.Test);
            Level = Zone.CreateLevel(0);

            Player.Spawn(Level);
            Player.FieldOfView.Update(Level);
        }

        public void Start()
        {
            if (Started)
            {
                return;
            }

            Round = 1;
            Message(
                "Welcome to [color=yellow]the Labyrinth[/color]. " +
                "Make your way to the bottom. Good luck!");
            Level.Scheduler.Advance();
            Started = true;
        }

        public void React(UserAction action)
        {
            LastCost = 0;
            ++Round;

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

        public void Message([NotNull] string message, bool important = false)
        {
            foreach (var listener in MessageListeners)
            {
                listener.OnMessage(message, Round, important);
            }
        }
    }
}
