using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

namespace Platformers
{
    public class UIManager : MonoBehaviour
    {
        public GameObject gameOverScreen;


        [SerializeField] private FirstPersonController playerController;

        public static bool enables = false;

        private void Awake()
        {
            if (gameOverScreen == null)
            {
                enabled = false; 
                return;
            }
            gameOverScreen.SetActive(false);

            if (playerController == null)
            {
                playerController = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
                
            }
        }

        // enables the game over screen when the player dies or starves
        public void OnEnable()
        {
                HealthManager.PlayerDead += ShowGameOverScreen;    
                StaminaManager.PlayerStarved += ShowGameOverScreen;
            
        }

        private void OnDisable()
        {
                HealthManager.PlayerDead -= ShowGameOverScreen;
                StaminaManager.PlayerStarved -= ShowGameOverScreen;
            
        }

        void ShowGameOverScreen()
        {
            if (gameOverScreen != null) // Check if the UI element still exists
            {
                enables = true;
                gameOverScreen.SetActive(true);
            }
            
            // Disable player controls when game over screen appears
            if (playerController != null)
            {
                playerController.SetControlsEnabled(false);
            }

        }
        // restarts the current scene
        public void RestartGame()
        {

            enables = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }
}