using JetBrains.Annotations;

namespace Labyrinth.Messages
{
    public interface IMessageListener
    {
        void OnMessage([NotNull] string message, int round, bool important);
    }
}
