using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Platformers
{
    public class ShopManager : MonoBehaviour
    {
        public int[,] shopItems = new int[6, 6];
        public float coins;
        public TextMeshProUGUI coinsTxt;

        // Assign this in the Inspector!
        [SerializeField] private FirstPersonController playerController;
        public ScrollRect itemScrollView;

        public static bool isShopOpen = false;
        public InventoryManager inventoryManager;
        public Button closeButton;

        void Awake()
        {
            // FPSController assignment
            if (playerController == null)
            {
                playerController = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
                
            }
            

            if (itemScrollView != null)
            {
                itemScrollView.gameObject.SetActive(false); // Ensure shop is closed at start
            }
            else
            {

                enabled = false; // Disable script if critical component is missing
            }

            if (coinsTxt == null)
            {
                Debug.LogError("ShopManager: coinsTxt is not assigned!", this);
                
            }
            // InventoryManager assignment so we can add items to inventory on purchase
            if (inventoryManager == null)
            {
                
                inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
                
            }
        }


        void Start()
        {
            UpdateCoinsText();

            shopItems[1, 0] = 0;
            shopItems[1, 1] = 1; // Item IDs or types
            shopItems[1, 2] = 2; // Fixed incorrect indexing from original code
            shopItems[1, 3] = 3;
            shopItems[1, 4] = 4;
            shopItems[1, 5] = 5;


            shopItems[2, 0] = 5;
            shopItems[2, 1] = 10; // Prices
            shopItems[2, 2] = 20;
            shopItems[2, 3] = 30; // Corrected original code (was [3,3] and [4,4])
            shopItems[2, 4] = 40;
            shopItems[2, 5] = 60;

            shopItems[3, 0] = 0;
            shopItems[3, 1] = 0; // Quantities (player's owned quantity, or shop's stock)
            shopItems[3, 2] = 0;
            shopItems[3, 3] = 0;
            shopItems[3, 4] = 0;
            shopItems[3, 5] = 0;
        }
        // Updates the coins text UI element
        void UpdateCoinsText()
        {
            if (coinsTxt != null)
            {
                coinsTxt.text = "Coins: " + coins.ToString(); // Changed "coins:" to "Coins:" for consistency
            }
        }
        // Method to handle buying an item
        public void Buy()
        {
            
            if (UnityEngine.EventSystems.EventSystem.current == null)
            {
                
                return;
            }
            // Get the currently selected button in the EventSystem 
            GameObject ButtonRef = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            if (ButtonRef == null)
            {
                
                return;
            }

            ButtonInfo buttonInfo = ButtonRef.GetComponent<ButtonInfo>();
            if (buttonInfo == null)
            {
                Debug.LogError("ShopManager: Selected GameObject for Buy does not have a ButtonInfo component.", ButtonRef);
                return;
            }
            // Get item ID and price from shopItems array and process purchase
            int itemID = buttonInfo.ItemID;
            float itemPrice = shopItems[2, itemID]; // Using itemID for price

            if (coins >= itemPrice)
            {
                coins -= itemPrice;
                shopItems[3, itemID] += 1; // Increment quantity
                
                if (inventoryManager != null)
                {
                    inventoryManager.AddItems(buttonInfo.item);
                }
                
                UpdateCoinsText();
                buttonInfo.QuantityText.text = shopItems[3, itemID].ToString();
            }
            
        }

        public void OpenShop()
        {
            if (itemScrollView != null) itemScrollView.gameObject.SetActive(true);

            if (playerController != null)
            {
                playerController.SetControlsEnabled(false); // Correct: Only disable controls once
            }


            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            isShopOpen = true;
            ShowCoinsText();
            
        }

        public void CloseShop()
        {
            if (itemScrollView != null) itemScrollView.gameObject.SetActive(false);

            if (playerController != null)
            {
                playerController.SetControlsEnabled(true); // Correct: Only enable controls once
            }

            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            isShopOpen = false;
            closeButton.gameObject.SetActive(false);
            HideCoinsText();

        }

        // Methods to show, hide, or toggle the coins text UI element
        public void ShowCoinsText()
        {
            if (coinsTxt != null)
            {
                coinsTxt.gameObject.SetActive(true);
            }
        }

        public void HideCoinsText()
        {
            if (coinsTxt != null)
            {
                coinsTxt.gameObject.SetActive(false);
            }
        }

        public void ToggleCoinsText()
        {
            if (coinsTxt != null)
            {
                bool isActive = coinsTxt.gameObject.activeSelf;
                coinsTxt.gameObject.SetActive(!isActive);
            }
        }
    }
}