using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
namespace Platformers
{
    public class HealthManager : MonoBehaviour
    {
        
        public Image healthBar;
        public float health = 100f;
        public InputAction Heals;
        public InputAction TakeDamages;
        public static event Action PlayerDead;
        public InventorySlot[] inventorySlots;
        public float maxHealth;
        public CharacterStatss CharacterStats;

        // sets initial max health value
        void Start()
        {
            CharacterStats = GameObject.Find("Characters").GetComponent<CharacterStatss>();
            maxHealth = CharacterStats.MaxHealth;
            health = maxHealth;

        }


        // enables and disables input actions

        public void OnEnable()
        {
            Heals.Enable();
            TakeDamages.Enable();
         
        }

        public void OnDisable()
        {
            Heals.Disable();
            TakeDamages.Disable();
            
        }

        // this section is for taking damage and healing

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0f) {
                health = 0f;
                
                PlayerDead?.Invoke();
                healthBar.fillAmount = 0f / maxHealth;
            }

            healthBar.fillAmount = health / maxHealth;


        }
        public void Heal(int amount)
        {
            health += amount;
            Debug.Log("Player health: " + health);
            if (health > maxHealth) health = maxHealth;
            healthBar.fillAmount = health / maxHealth;
        }
    }
}
