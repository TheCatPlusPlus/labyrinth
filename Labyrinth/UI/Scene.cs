using Labyrinth.UI.Input;

namespace Labyrinth.UI
{
    public abstract class Scene
    {
        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual void React(Event @event)
        {
        }

        public virtual void Draw()
        {
        }
    }
}
