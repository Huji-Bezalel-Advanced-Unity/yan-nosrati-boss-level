using System.Collections;
using System.Threading.Tasks;
using Managers;
using UnityEngine;

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
            CoreManager.Instance.MonoBehaviourRunner.StartCoroutine(DoStunForSeconds(entity));
        }

        private IEnumerator DoStunForSeconds(Entity entity)
        {
            entity.animator.SetTrigger("Stunned");
            entity.stunned = true;
            yield return new WaitForSeconds(Duration);
            entity.stunned = false;
        }
    }
}