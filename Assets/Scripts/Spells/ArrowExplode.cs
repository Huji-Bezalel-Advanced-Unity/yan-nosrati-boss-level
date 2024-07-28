using System;
using System.Collections;
using UnityEngine;

namespace Spells
{
    public class ArrowExplode : MonoBehaviour
    {
        [SerializeField] private ParticleSystem explosion;

        public void Init(Vector3 position)
        {
            explosion.transform.position = position;
            StartCoroutine(Explode());
        }
        
        private IEnumerator Explode()
        {
            explosion.Play();

            yield return new WaitForSeconds(explosion.main.duration);

            explosion.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            explosion.gameObject.SetActive(false);
        }
    }
}