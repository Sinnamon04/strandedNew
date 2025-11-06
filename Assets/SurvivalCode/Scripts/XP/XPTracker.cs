using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Platformers
{
    public class XPTracker : MonoBehaviour
    {


        [SerializeField] BaseXPTranslations XPTranslationType;
        BaseXPTranslations XPTranslation;
        [SerializeField] TextMeshProUGUI CurrentLevelText;
        [SerializeField] TextMeshProUGUI CurrentXPText;
        [SerializeField] TextMeshProUGUI XPToNextLevelText;
        [SerializeField] UnityEvent<int,int> OnLevelChanged = new UnityEvent<int, int> ();
        public int level = 1;

        // Start is called before the first frame update

        private void Awake()
        {
            XPTranslation = ScriptableObject.Instantiate(XPTranslationType);
        }
        // Method to add XP and handle level changes
        public void AddXP(int amount)
        {
            int previousLevel = XPTranslation.CurrentLevel;
            if (XPTranslation.AddXP(amount)) {
                OnLevelChanged.Invoke(previousLevel, XPTranslation.CurrentLevel);
            }
            UpdateUI();
        }
        // Method to set level directly and handle level changes
        public void SetLevel(int level) {
            int previousLevel = XPTranslation.CurrentLevel;

            XPTranslation.SetLevel(level);
            // Check for level change and invoke the method to change level if changed 
            if (previousLevel != XPTranslation.CurrentLevel) {
                OnLevelChanged.Invoke(previousLevel, XPTranslation.CurrentLevel);
            }
            UpdateUI();
        }
        // sets current XP directly and the current level
        void Start()
        {
            UpdateUI();
            OnLevelChanged.Invoke(0, XPTranslation.CurrentLevel);
        }
        // Method to update the UI elements and display current level and XP and makes sure to show MAX LEVEL if at max level
        private void UpdateUI()
        {
            CurrentLevelText.text = "Level: " + XPTranslation.CurrentLevel;
            level = XPTranslation.CurrentLevel;
            CurrentXPText.text = "XP: " + XPTranslation.CurrentXP;
            if (!XPTranslation.IsMaxLevel)
            {
                XPToNextLevelText.text = "XP to next level: " + XPTranslation.XPToNextLevel;
              }
            else
            {
                XPToNextLevelText.text = "XP to next level: MAX LEVEL";
            }
        }
    }
}
