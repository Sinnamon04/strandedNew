using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Platformers
{
    public class CharacterStatss : MonoBehaviour
    {
        // Character stats like stamina, health, strength, defense etc.
        [SerializeField] public int StaminaToHealthConversion = 5;
        [SerializeField] public int BaseStaminaPerLevel = 5;
        [SerializeField] public int BaseStaminaOffset = 10;

        [SerializeField] public int BaseDefensePerLevel = 3;
        [SerializeField] public int BaseDefenseOffset = 2;

        [SerializeField] public int BaseHealthPerLevel = 20;
        [SerializeField] public int BaseHealthOffset = 100;

        [SerializeField] public int BaseStrengthPerLevel = 2;
        [SerializeField] public int BaseStrengthOffset = 5;
        [SerializeField] public TextMeshProUGUI StaminaText;
        [SerializeField] public TextMeshProUGUI HealthText;
        [SerializeField] public TextMeshProUGUI StrengthText;
        [SerializeField] public TextMeshProUGUI DefenseText;
        [SerializeField] EquipmentSlot[] EquipmentSlots;


        // Base stats that get modified by level and equipment
        public int BaseStamina { get;  set; } = 0;
        public int BaseHealth { get; set; } = 0;

        public int BaseStrength { get; set; } = 0;

        public int BaseDefense { get; set; } = 0;


        // Calculated stats that include base stats and equipment bonuses
        public int Stamina
        {
            get
            {
                return BaseStamina;
            }

        }

        public int MaxHealth {
        
            get {
                return (Stamina * StaminaToHealthConversion) + BaseHealth;
            }

        }

        public int Strength
        {
            get
            {
                return BaseStrength;
            }
        }

        public int Defense
        {
            get
            {
                return BaseDefense;
            }
        }

        // Method to update stats when the character levels up
        public void OnUpdateLevel(int previousLevel,int currentLevel)
        {
            BaseStamina = BaseStaminaPerLevel * currentLevel + BaseStaminaOffset;
            StaminaText.text = $"Stamina: {Stamina}";
            BaseHealth = BaseHealthPerLevel * currentLevel + BaseHealthOffset;
            HealthText.text = $"Health: {MaxHealth}";
            BaseStrength = BaseStrengthPerLevel * currentLevel + BaseStrengthOffset;
            StrengthText.text = $"Strength: {BaseStrength}";
            BaseDefense = BaseDefensePerLevel * currentLevel + BaseDefenseOffset;
            DefenseText.text = $"Defense: {BaseDefense}";
            


        }
    }
}
