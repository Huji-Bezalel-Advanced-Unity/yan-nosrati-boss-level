using System;
using System.Collections.Generic;
using Bosses;
using DefaultNamespace.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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


        public void Awake()
        {
            if (instance == null) instance = this;

            _phaseToImageMap = new Dictionary<Phase, Sprite>()
            {
                { Phase.MediumHealth, mediumHealthSprite },
                { Phase.LowHealth, lowHealthSprite }
            };
        }
        
        void OnEnable()
        {
            Player.OnPlayerChangePhase += ChangeMapImage;

            EventManager.Instance.AddListener(EventNames.OnGameOver, GameOver);
        }

        private void OnDisable()
        {
            Player.OnPlayerChangePhase -= ChangeMapImage;

            EventManager.Instance.RemoveListener(EventNames.OnGameOver, GameOver);
        }

        private void GameOver(object obj)
        {
            print("called ambiance change");
            print(obj);
            if (obj is Action action)
            {
                print("object is action");
                StartCoroutine(ambience.Brighten(action));
            }
        }


        public void ChangeMapImage(Phase phase)
        {
            _renderer.sprite = _phaseToImageMap[phase];
        }
    }
}