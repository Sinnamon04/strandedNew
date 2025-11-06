using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Platformers
{
    public class CharacterStatss : MonoBehaviour
    {
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
        public int BaseStamina { get;  set; } = 0;
        public int BaseHealth { get; set; } = 0;

        public int BaseStrength { get; set; } = 0;

        public int BaseDefense { get; set; } = 0;

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
