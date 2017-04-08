namespace Labyrinth.Entities.Time
{
    public interface IEnergy
    {
        void Recharge();
        void Use(int cost);
    }
}
