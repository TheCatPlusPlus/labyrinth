using System.Threading;

namespace Labyrinth.UI.Input.Auto
{
    public abstract class AutoAction : EventLoop
    {
        private bool _isRunning = true;
        protected override bool IsRunning => _isRunning;

        protected override Event Wait()
        {
            if (!Event.HasNext())
            {
                Thread.Sleep(Const.AnimFrameTime);
            }

            if (Event.HasNext())
            {
                _isRunning = false;
            }

            return null;
        }

        protected override void Draw(Scene scene)
        {
            if (!Execute(scene))
            {
                _isRunning = false;
            }

            base.Draw(scene);
        }

        protected abstract bool Execute(Scene scene);
    }
}
