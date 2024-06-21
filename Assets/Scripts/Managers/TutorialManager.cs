using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class TutorialManager
    {
        public static TutorialManager Instance { get; private set; }
        private GameObject _tutorialPanel;
        private Animator _animator;
        private TextMeshProUGUI _text;
        private Image _image;
        private Dictionary<KeyCode, ValueTuple<int,string>> spellToDescriptionMap;
        private bool running;


        
        public TutorialManager(GameObject tutorialPanel)
        {
            if (Instance == null)
            {
                Instance = this;
            }

            running = false;
            _tutorialPanel = tutorialPanel;
            _image = tutorialPanel.transform.GetChild(0).GetComponent<Image>();
            _tutorialPanel.SetActive(false);
            _animator = _tutorialPanel.transform.GetChild(0).GetComponent<Animator>();
            _text = _tutorialPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            spellToDescriptionMap = new Dictionary<KeyCode,ValueTuple<int,string>>()
                { { KeyCode.Q, (1000,"WARRIORS") }, { KeyCode.W, (300,"FAIRY") }, { KeyCode.R, (100,"DIVINE") } };
        }

        public async Task RunSpellTutorial(KeyCode spell, Vector3 position)
        {
            if (running) return;
            running = true;
            _image.transform.position = position;
            _text.text = spellToDescriptionMap[spell].Item2;
            await Task.Delay(spellToDescriptionMap[spell].Item1);
            _tutorialPanel.SetActive(true);
            _animator.SetTrigger("PopIn");
            await Task.Delay(8000);
            _animator.SetTrigger("PopOut");
            await Task.Delay(300);
            _tutorialPanel.SetActive(false);
            running = false;

        }

    }
}