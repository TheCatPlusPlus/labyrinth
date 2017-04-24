using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Labyrinth.UI.Input
{
    public sealed class KeyMap
    {
        public static readonly KeyMap Game;
        public static readonly KeyMap Menu;
        private readonly Dictionary<Key, UserAction> _defaultKeys;
        private readonly Dictionary<Key, UserAction> _userKeys;

        public UserAction? this[[NotNull] Key key]
        {
            get
            {
                UserAction action;

                if (_userKeys.TryGetValue(key, out action))
                {
                    return action;
                }

                if (_defaultKeys.TryGetValue(key, out action))
                {
                    return action;
                }

                return null;
            }
        }

        static KeyMap()
        {
            var common = new KeyMap();
            common.Bind(UserAction.Cancel, "escape");
            common.Bind(UserAction.Confirm, "enter", "num-enter");

            Menu = common.Clone();
            Menu.Bind(UserAction.PreviousItem, "up", "num-8");
            Menu.Bind(UserAction.NextItem, "down", "num-2");

            Game = common.Clone();
            Game.Bind(UserAction.Quit, "ctrl q");
            Game.Bind(UserAction.SaveQuit, "ctrl s");
            Game.Bind(UserAction.MoveNorth, "num-8", "up");
            Game.Bind(UserAction.MoveSouth, "num-2", "down");
            Game.Bind(UserAction.MoveWest, "num-4", "left");
            Game.Bind(UserAction.MoveEast, "num-6", "right");
            Game.Bind(UserAction.MoveNorthWest, "num-7");
            Game.Bind(UserAction.MoveSouthWest, "num-1");
            Game.Bind(UserAction.MoveNorthEast, "num-9");
            Game.Bind(UserAction.MoveSouthEast, "num-3");
            Game.Bind(UserAction.Wait, "num-5", ".");
            Game.Bind(UserAction.TakeStairs, "<", ">");
            Game.Bind(UserAction.ShowInventory, "i");
            Game.Bind(UserAction.ShowCharacter, "@");
            Game.Bind(UserAction.DebugMenu, "f12");
            Game.Bind(UserAction.MoveTo, "mouse-left");
        }

        public KeyMap()
        {
            _userKeys = new Dictionary<Key, UserAction>();
            _defaultKeys = new Dictionary<Key, UserAction>();
        }

        public void Bind(UserAction action, [NotNull] string key, [NotNull] params string[] rest)
        {
            DoBind(action, key, true);
            foreach (var other in rest)
            {
                DoBind(action, other, true);
            }
        }

        public void BindUser(UserAction action, [NotNull] string key, [NotNull] params string[] rest)
        {
            DoBind(action, key, false);
            foreach (var other in rest)
            {
                DoBind(action, other, false);
            }
        }

        [NotNull]
        public KeyMap Clone()
        {
            var clone = new KeyMap();

            foreach (var pair in _defaultKeys)
            {
                clone._defaultKeys.Add(pair.Key, pair.Value);
            }

            foreach (var pair in _userKeys)
            {
                clone._userKeys.Add(pair.Key, pair.Value);
            }

            return clone;
        }

        private void DoBind(UserAction action, [NotNull] string spec, bool isDefault)
        {
            var key = KeyDatabase.Parse(spec);
            var map = isDefault ? _defaultKeys : _userKeys;

            if (map.ContainsKey(key))
            {
                throw new ArgumentException($"Failed to bind '{key}' to {action}: already bound to {map[key]}");
            }

            map[key] = action;
        }
    }
}
