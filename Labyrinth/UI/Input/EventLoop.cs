using BearLib;

using JetBrains.Annotations;

namespace Labyrinth.UI.Input
{
    public abstract class EventLoop
    {
        protected abstract bool IsRunning { get; }

        public virtual void Run()
        {
            while (IsRunning)
            {
                var scene = State.Scene;

                Terminal.Clear();
                Draw(scene);
                Terminal.Refresh();

                var @event = Wait();
                React(scene, @event);
            }
        }

        [CanBeNull]
        protected virtual Event Wait()
        {
            return Event.ReadNext();
        }

        protected virtual void Draw([NotNull] Scene scene)
        {
            scene.Draw();
        }

        protected virtual void React([NotNull] Scene scene, [CanBeNull] Event @event)
        {
            if (@event != null)
            {
                scene.React(@event);
            }
        }
    }
}