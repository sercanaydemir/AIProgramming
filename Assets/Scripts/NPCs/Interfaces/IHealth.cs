namespace NPCs.Interfaces
{
    public interface IHealth
    {
        float Health { get; set; }
        void TakeDamage(float damage);
        void Heal(float amount);
        bool IsDead();
    }

}