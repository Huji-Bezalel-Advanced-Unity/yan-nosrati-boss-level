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

        private Image activeImage;
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

        public void RestartGame()
        {
            // maybe
        }

        // will be called from a button press
        public void ExitToMainMenu()
        {
            activeImage.gameObject.SetActive(false);
            GameManager.Instance.ResumeTime();
            SceneManager.LoadScene("CharacterLoader");
        }

        // will be called as an animation event
        public void MenuAppeared()
        {
            GameManager.Instance.StopTime();
        }

        private void OpenEndMenu()
        {
            if (GameManager.Instance.GetGameState() == GameState.Won)
            {
                wonImage.gameObject.SetActive(true);
                activeImage = wonImage;
            }
            else
            {
                lostImage.gameObject.SetActive(true);
                activeImage = lostImage;
            }

            animator.SetTrigger("Appear");
        }
    }
}