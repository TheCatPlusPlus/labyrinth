using System.Diagnostics;

using JetBrains.Annotations;

using Labyrinth.UI;

using Pcg;

namespace Labyrinth
{
    public static class State
    {
        [CanBeNull]
        private static Game _game;

        public static PcgRandom Rng { get; private set; }
        public static Scene Scene { get; private set; }
        public static bool IsRunning { get; private set; }
        public static bool IsGameLoaded => _game != null;

        [NotNull]
        public static Game Game
        {
            get
            {
                Debug.Assert(_game != null, "State.Game used before game is loaded");
                return _game;
            }
        }

        static State()
        {
            IsRunning = true;
            Rng = new PcgRandom();
        }

        public static void SignalExit()
        {
            IsRunning = false;
        }

        public static void SetScene<T>()
            where T : Scene, new()
        {
            Scene?.OnExit();
            Scene = new T();
            Scene.OnEnter();
        }

        public static void NewGame([NotNull] string playerName)
        {
            if (IsGameLoaded)
            {
                DiscardGame();
            }

            _game = new Game(playerName);
        }

        public static void LoadGame()
        {
            _game = SavedGame.Restore();
        }

        public static void SaveGame()
        {
            SavedGame.Persist(Game);
            SignalExit();
        }

        public static void DiscardGame()
        {
            _game = null;
            SavedGame.Discard();
        }
    }
}
