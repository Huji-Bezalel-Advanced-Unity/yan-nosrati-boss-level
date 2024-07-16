using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DefaultNamespace.Utilities
{
    public class AmbienceChanger : MonoBehaviour
    {
        [SerializeField] private Volume postProcessVolume;
        [SerializeField] private float minThreshold = 0.5f;
        private float brightenDuration = 0.7f;
        private float initialThreshold;
        private Bloom bloom;

        void Start()
        {
            if (postProcessVolume != null && postProcessVolume.profile != null)
            {
                if (postProcessVolume.profile.TryGet<Bloom>(out bloom))
                {
                    initialThreshold = bloom.threshold.value;
                }
                else
                {
                    Debug.LogError("Bloom component not found in Volume profile.");
                }
            }
            else
            {
                Debug.LogError("PostProcessVolume or Volume profile is not assigned.");
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