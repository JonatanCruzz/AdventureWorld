using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventureWorld.Prueba
{
    public class CloseUI : MonoBehaviour
    {
        public Canvas canvas;

        private void Reset()
        {
            canvas = GetComponentInChildren<Canvas>();
        }

        public void OnReturnToMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }

        public void OnExitGame()
        {
            Application.Quit();
        }
    }
}