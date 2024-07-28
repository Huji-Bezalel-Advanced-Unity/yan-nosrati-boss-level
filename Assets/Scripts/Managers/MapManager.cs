using System;
using System.Collections.Generic;
using Bosses;
using player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Utilities;
using Warriors;

namespace Managers
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager instance;

        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Sprite mediumHealthSprite;
        [SerializeField] private Sprite lowHealthSprite;
        [SerializeField] private AmbienceChanger ambience;

        private Dictionary<Phase, Sprite> _phaseToImageMap;
        private Volume _volume;


        public void Awake()
        {
            if (instance == null) instance = this;

            _phaseToImageMap = new Dictionary<Phase, Sprite>()
            {
                { Phase.MediumHealth, mediumHealthSprite },
                { Phase.LowHealth, lowHealthSprite }
            };
        }

        public void Init(Volume volume)
        {
            ambience.Init(volume);
        }

        void OnEnable()
        {
            EventManager.Instance.AddListener(EventNames.OnPlayerChangePhase, ChangeMapImage);
            EventManager.Instance.AddListener(EventNames.OnGameOver, GameOver);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener(EventNames.OnPlayerChangePhase, ChangeMapImage);
            EventManager.Instance.RemoveListener(EventNames.OnGameOver, GameOver);
        }

        private void GameOver(object obj)
        {
            if (obj is Action action)
            {
                StartCoroutine(ambience.Brighten(action));
            }
        }


        private void ChangeMapImage(object obj)
        {
            if (obj is Phase phase)
            {
                _renderer.sprite = _phaseToImageMap[phase];
            }
        }
    }
}