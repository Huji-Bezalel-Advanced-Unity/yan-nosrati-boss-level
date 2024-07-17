using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DefaultNamespace.Utilities
{
    public class AmbienceChanger : MonoBehaviour
    {
        private Volume postProcessVolume;
        [SerializeField] private float minThreshold = 0.5f;
        private float brightenDuration = 0.7f;
        private float initialThreshold;
        private Bloom bloom;

        public void Init(Volume volume)
        {
            postProcessVolume = volume;
            if (postProcessVolume != null && postProcessVolume.profile != null)
            {
                if (postProcessVolume.profile.TryGet<Bloom>(out bloom))
                {
                    initialThreshold = bloom.threshold.value;
                }
            }
        }

        public IEnumerator Brighten(Action callback)
        {
            float timeElapsed = 0f;
            while (timeElapsed < brightenDuration)
            {
                timeElapsed += Time.deltaTime;
                bloom.threshold.value = Mathf.Lerp(initialThreshold, minThreshold, timeElapsed / brightenDuration);
                yield return null;
            }

            bloom.threshold.value = minThreshold;
            callback?.Invoke();
        }
    }
}