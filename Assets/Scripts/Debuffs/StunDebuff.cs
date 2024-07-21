using System.Threading.Tasks;

namespace Debuffs
{
    public class StunDebuff : Debuff
    {
        public float Duration;

        public StunDebuff(float duration)
        {
            Duration = duration;
        }

        public void Apply(Entity entity)
        {
            DoStunForSeconds(entity);
        }

        private async void DoStunForSeconds(Entity entity)
        {
            entity.animator.SetTrigger("Stunned");
            entity.stunned = true;
            await Task.Delay((int)Duration * 1000);
            entity.stunned = false;
        }
    }
}