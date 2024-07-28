using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Managers;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;


        [SerializedDictionary("SoundName", "Sound")]
        public SerializedDictionary<SoundName, AudioClip> gameSounds;
        
        [SerializeField] private AudioSource src;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void PlaySound(SoundName soundName)
        {
            if (GameManager.Instance.GetGameState() == GameState.Played &&
                gameSounds.TryGetValue(soundName, out AudioClip sound))
            {
                src.PlayOneShot(sound);
            }
        }
    }

    public enum SoundName
    {
        None = 0,
        ArrowShoot = 1,
        ArrowHit = 2,
        SpecialArrowHit = 3,
        WarriorSummon = 4,
        SwordHit = 5,
        WinSound = 6,
        LoseSound = 7,
        RockHit = 8,
        SkeletonSummon = 9,
        BowUpgrade = 10,
    }
}