namespace Debuffs
{
    public class DamageDebuff : Debuff
    {
        public int Damage { get; private set; }

        public DamageDebuff(int damage)
        {
            Damage = damage;
        }

        public void Apply(Entity entity)
        {
            entity.ChangeHealth(entity, Damage);
        }
    }
}