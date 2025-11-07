using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // Required for Image and Slider components
using System;
namespace Platformers
{
    public class StaminaManager : MonoBehaviour
    {
        [Header("UI")]
        public Image hungerBar; // Assign your UI Image here

        [Header("Hunger Settings")]
        [Range(0, 100)] // Clamp hunger value in inspector
        public float hunger = 100f; 
        public float maxHunger = 100f; 
       

        [Header("Hunger Drain")]
        public int hungerDrainPerTick = 1; 
        public float hungerTickInterval = 2.0f; 

        [Header("Health Link (Optional)")]

        public HealthManager playerHealthManager;
        public int healthDamagePerTick = 5; 
        public float healthDamageInterval = 1.0f; 

        [Header("Input Actions")]
        public InputAction rejuvenation; 
        public InputAction starvation;   

        private bool isHungerDraining = false; 
        private bool isHealthDraining = false;


        // Updates hunger and starts hunger drain on start
        void Start()
        {
            hunger = maxHunger; 
            UpdateHungerUI(); 

            
            StartContinuousHungerDrain();

            if (playerHealthManager == null)
            {
                playerHealthManager = FindFirstObjectByType<HealthManager>();
                
            }
        }

        

        public void OnDisable()
        {
          
            
            CancelInvoke();
            isHungerDraining = false;
            isHealthDraining = false;
        }



        // --- Hunger and Health Drain Logic ---
        public void StartContinuousHungerDrain()
        {
            if (!isHungerDraining)
            {
                InvokeRepeating("ApplyHungerDrainTick", hungerTickInterval, hungerTickInterval);
                isHungerDraining = true;
                
            }
        }
        // Applies hunger drain and checks for starvation to start health damage
        void ApplyHungerDrainTick()
        {
            ChangeHunger(-hungerDrainPerTick); 

            
            if (hunger <= 0)
            {
                
                    hunger = 0f;

                    
                    hungerBar.fillAmount = 0f / 100f;
                
                
                if (playerHealthManager != null && !isHealthDraining)
                {
                    InvokeRepeating("ApplyHealthDamageTick", healthDamageInterval, healthDamageInterval);
                    isHealthDraining = true;
                    
                }
            }
            else 
            {
                if (isHealthDraining)
                {
                    CancelInvoke("ApplyHealthDamageTick");
                    isHealthDraining = false;
                    
                }
            }
        }

        // Applies health damage when starving
        void ApplyHealthDamageTick()
        {
            if (playerHealthManager != null)
            {
                playerHealthManager.TakeDamage(healthDamagePerTick);
            }
            else
            {
                
                CancelInvoke("ApplyHealthDamageTick");
                isHealthDraining = false;
                
            }
        }



        // reduces or increases hunger by specified amount
        public void ChangeHunger(float amount)
        {
            hunger += amount;
            hunger = Mathf.Clamp(hunger, 0, maxHunger); 
            UpdateHungerUI();
            
        }

        // Updates the hunger UI bar
        void UpdateHungerUI()
        {
            if (hungerBar != null)
            {
                hungerBar.fillAmount = hunger / maxHunger;
            }
            
        }
    }
}