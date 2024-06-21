﻿using System;
using System.Collections.Generic;
using Bosses;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager instance;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField]private Sprite mediumHealthSprite;
        [SerializeField]private Sprite lowHealthSprite;
        private Dictionary<Phase, Sprite> phaseToImageMap;

        public void Awake()
        {
            if (instance == null) instance =this;
            
            phaseToImageMap = new Dictionary<Phase, Sprite>()
            {
                { Phase.MediumHealth, mediumHealthSprite },
                { Phase.LowHealth, lowHealthSprite }
            };
        }

        private void OnEnable()
        {
            Player.OnPlayerChangePhase += ChangeMapImage;
        }

        public void ChangeMapImage(Phase phase)
        {
            _renderer.sprite = phaseToImageMap[phase];

        }
    }
}