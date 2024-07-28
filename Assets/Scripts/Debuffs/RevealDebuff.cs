using Managers;
using UnityEngine;
using Utilities;

namespace Debuffs
{
    public class RevealDebuff : Debuff
    {
        public int Duration { get; private set; }

        public RevealDebuff(int duration)
        {
            Duration = duration;
        }

        public void Apply(Entity entity)
        {
            StartFade(entity.renderer);
        }


        // Example of how to call the async function
        public void StartFade(Renderer renderer)
        {
            if (renderer != null && renderer.material.color.a < 1f)
            {
                 CoreManager.Instance.MonoBehaviourRunner.StartCoroutine(Util.DoFadeLerp(renderer, renderer.material.color.a, 1f,
                    Duration, () =>
                    {
                        CoreManager.Instance.MonoBehaviourRunner.StartCoroutine(Util.DoFadeLerp(renderer, 1, 0, Duration, null));
                    }));
            }
        }
    }

  
}