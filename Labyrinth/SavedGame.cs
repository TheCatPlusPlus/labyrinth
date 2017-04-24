using System;

namespace Labyrinth
{
    public static class SavedGame
    {
        public static bool Exists()
        {
            return false;
        }

        public static Game Restore()
        {
            // TODO
            throw new NotImplementedException();
        }

        public static void Persist(Game game)
        {
            // TODO
        }

        public static void Discard()
        {
            // TODO
        }
    }
}
