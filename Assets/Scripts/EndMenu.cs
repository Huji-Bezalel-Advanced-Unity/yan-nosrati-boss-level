using System;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class EndMenu : MonoBehaviour
    {
        [SerializeField] private Image lostImage;
        [SerializeField] private Image wonImage;
        private Animator animator;
        private void Awake()
        {
            GameManager.Instance.EndGame += OpenEndMenu;
            animator = GetComponent<Animator>();
        }

        private void OnDestroy()
        {
            GameManager.Instance.EndGame -= OpenEndMenu;
        }

        private void OpenEndMenu()
        {
            if (GameManager.Instance.wonGame)
            {
                wonImage.gameObject.SetActive(true);
            }
            else
            {
                lostImage.gameObject.SetActive(true);
            }
            animator.SetTrigger("Appear");
        }

        public void RestartGame()
        {
        }
        // will be called from a button press
        public void ExitToMainMenu()
        {
            print("CALLED MAIN MENU");
            SceneManager.LoadScene("MainMenu");
        }

        // will be called as an animation event
        public void MenuAppeared()
        {
            GameManager.Instance.StopTime();
        }
        
    }
}