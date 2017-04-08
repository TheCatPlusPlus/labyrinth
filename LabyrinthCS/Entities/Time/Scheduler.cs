using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using Labyrinth.Maps;
using Labyrinth.Utils;

namespace Labyrinth.Entities.Time
{
    public sealed class Scheduler
    {
        private readonly Level _level;
        private readonly LinkedList<Actor> _queue;
        private readonly IEnumerator _loop;

        private int _freeTurns;

        public Scheduler(Level level)
        {
            _level = level;
            _queue = new LinkedList<Actor>();
            _loop = Coroutine().GetEnumerator();
        }

        public void Advance()
        {
            _loop.MoveNext();
        }

        [ItemCanBeNull]
        private IEnumerable Coroutine()
        {
            while (true)
            {
                if (_queue.Count == 0)
                {
                    Log.Verbose(Log.Category.Scheduler, $"Scheduler: no monsters");
                    yield return null;
                }

                Actor actor;

                if (_freeTurns > 0)
                {
                    // free turns when first entering the level
                    actor = State.Game.Player;
                }
                else
                {
                    actor = _queue.First.Value;
                    _queue.RemoveFirst();

                    if (!actor.IsAlive)
                    {
                        continue;
                    }

                    _queue.AddLast(actor);
                }

                var energy = (IEnergy)actor;

                if (actor is Player player)
                {
                    Log.Verbose(Log.Category.Scheduler, $"Scheduler: player turn ({player.Energy}, free {_freeTurns})");

                    while ((player.Energy > 0) || (_freeTurns > 0))
                    {
                        yield return null;

                        if (State.Game.LastCost >= 0)
                        {
                            energy.Use(State.Game.LastCost);

                            if (_freeTurns > 0)
                            {
                                --_freeTurns;
                            }
                        }
                    }

                    Log.Verbose(Log.Category.Scheduler, $"Scheduler: player turn end ({player.Energy})");
                }

                if (actor is Monster monster)
                {
                    while ((monster.Energy > 0) && monster.IsAlive)
                    {
                        Log.Verbose(Log.Category.Scheduler, $"Scheduler: {monster} turn ({monster.Energy})");

                        var cost = monster.Act();
                        if (cost >= 0)
                        {
                            energy.Use(cost);
                        }

                        Log.Verbose(Log.Category.Scheduler, $"Scheduler: {monster} turn end ({monster.Energy})");
                    }
                }

                energy.Recharge();
            }

            // ReSharper disable once IteratorNeverReturns
        }

        public void OnAdd(Actor actor)
        {
            if (actor is Player)
            {
                _freeTurns = 1;
            }

            _queue.AddLast(actor);
        }

        public void OnRemove(Actor actor)
        {
            _queue.Remove(actor);
        }
    }
}
