namespace RehvidGames.Interfaces
{
    public interface IHealth
    {
        public void ReceiveDamage(float damage);
        public bool IsDead();
    }
}