using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Managers;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class CastManagerUI : MonoBehaviour
    {

        [SerializeField] private GameObject spellsPanel;
        [SerializedDictionary("Spell", "Image")]
        public AYellowpaper.SerializedCollections.SerializedDictionary<Spell, Image> ImageBySpellMap;

        void OnEnable()
        {
            EventManager.Instance.AddListener(EventNames.OnSpellCast, UpdateUI);
            GameManager.Instance.EndGame += DisableSpellUI;
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListener(EventNames.OnSpellCast, UpdateUI);
            GameManager.Instance.EndGame -= DisableSpellUI;

        }

        private void DisableSpellUI()
        {
            spellsPanel.SetActive(false);
        }


        private void UpdateUI(object obj)
        {
            if (obj is (Spell spell))
            {
                StartCoroutine(Util.DoFillLerp(ImageBySpellMap[spell], 1, 0, spell.GetCooldown()));
            }
        }
    }
}