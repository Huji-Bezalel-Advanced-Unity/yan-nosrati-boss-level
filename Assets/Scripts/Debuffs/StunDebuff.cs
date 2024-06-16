namespace Debuffs
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;

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

        private async Task DoStunForSeconds(Entity entity)
        {
            entity.stunned = true;
            await Task.Delay((int)Duration*1000);
            entity.stunned = false;
        }
    }
}