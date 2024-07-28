using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Managers;
using UnityEngine;

namespace Debuffs
{
    public class SlowDebuff : Debuff
    {
        public int Slow { get; private set; }
        public float Duration;

        public SlowDebuff(int slowPrecent, float duration)
        {
            Slow = slowPrecent;
            Duration = duration;
        }

        public void Apply(Entity entity)
        {
            CoreManager.Instance.MonoBehaviourRunner.StartCoroutine(DoSlowForSeconds(entity));
        }

        private IEnumerator DoSlowForSeconds(Entity entity)
        {
            float speed = entity.moveSpeed;
            entity.moveSpeed = (float)(100 - Slow) / 100 * entity.moveSpeed;
            yield return new WaitForSeconds(Duration);
            entity.moveSpeed = speed;
        }
    }
}