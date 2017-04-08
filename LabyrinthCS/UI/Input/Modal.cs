using JetBrains.Annotations;

namespace Labyrinth.UI.Input
{
    public abstract class Modal<T> : EventLoop
    {
        private bool _isOpen = true;
        protected override bool IsRunning => _isOpen;

        public T Result { get; private set; }

        protected void Close(T result)
        {
            _isOpen = false;
            Result = result;
        }

        protected override void Draw(Scene scene)
        {
            base.Draw(scene);
            DrawModal();
        }

        protected override void React(Scene scene, Event @event)
        {
            if ((@event != null) && @event is KeyEvent key)
            {
                ReactModal(key);
            }
        }

        protected abstract void DrawModal();
        protected abstract void ReactModal([NotNull] KeyEvent @event);
    }
}
