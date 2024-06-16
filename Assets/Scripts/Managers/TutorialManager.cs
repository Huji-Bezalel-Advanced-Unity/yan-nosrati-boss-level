using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Managers;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class TutorialManager
    {
        public static TutorialManager Instance { get; private set; }
        private GameObject _tutorialPanel;
        private Animator _animator;
        private TextMeshProUGUI _text;
        private Dictionary<KeyCode, ValueTuple<int,string>> spellToDescriptionMap;

        
        public TutorialManager(GameObject tutorialPanel)
        {
            if (Instance == null)
            {
                Instance = this;
            }
            _tutorialPanel = tutorialPanel;
            _tutorialPanel.SetActive(false);
            _animator = _tutorialPanel.transform.GetChild(0).GetComponent<Animator>();
            _text = _tutorialPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            spellToDescriptionMap = new Dictionary<KeyCode,ValueTuple<int,string>>()
                { { KeyCode.Q, (1000,"WARRIORS") }, { KeyCode.W, (300,"FAIRY") }, { KeyCode.R, (100,"DIVINE") } };
        }

        public async Task runSpellTutorial(KeyCode spell, Vector3 position)
        {
            _tutorialPanel.transform.position = position;
            _text.text = spellToDescriptionMap[spell].Item2;
            await Task.Delay(spellToDescriptionMap[spell].Item1);
            _tutorialPanel.SetActive(true);
            _animator.SetTrigger("PopIn");
            Time.timeScale = 0.1f;
            await Task.Delay(8000);
            _animator.SetTrigger("PopOut");
           await Task.Delay(300);
            _tutorialPanel.SetActive(false);
            Time.timeScale = 1f;

        }

    }
}