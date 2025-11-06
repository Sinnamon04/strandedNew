using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformers
{
    public class InventoryManager : MonoBehaviour
    {


        // Start is called before the first frame update

        public InventorySlot[] inventorySlots;
        public GameObject inventoryPrefab;
        public int maxItemCount = 1;
        int selectedSlot = -1;
        
        private Camera mainCamera;
        [SerializeField] private Transform weaponHolder;

        [SerializeField] private InputActionAsset Hotbar;

        [HideInInspector] private InputAction hotbar1Action;
        [HideInInspector] private InputAction hotbar2Action;
        [HideInInspector] private InputAction hotbar3Action;
        [HideInInspector] private InputAction hotbar4Action;
        [HideInInspector] private InputAction hotbar5Action;
        private GameObject currentWeaponObject = null;
        [SerializeField] private WeaponSwitching weaponSwitcher;

        public void Awake()
        {
            hotbar1Action = Hotbar.FindActionMap("UI").FindAction("Hotbar");
            hotbar2Action = Hotbar.FindActionMap("UI").FindAction("Hotbar1");
            hotbar3Action = Hotbar.FindActionMap("UI").FindAction("Hotbar2");
            hotbar4Action = Hotbar.FindActionMap("UI").FindAction("Hotbar3");
            hotbar5Action = Hotbar.FindActionMap("UI").FindAction("Hotbar4");
            mainCamera = Camera.main;

        }

        private void Start()
        {

            ChangeSelectedSlot(0);
        }

        public void OnEnable()
        {
            
            hotbar1Action?.Enable();
            hotbar2Action?.Enable();
            hotbar3Action?.Enable();
            hotbar4Action?.Enable();
            hotbar5Action?.Enable();
            hotbar1Action.performed += ctx => ChangeSelectedSlot(0);
            hotbar2Action.performed += ctx => ChangeSelectedSlot(1);
            hotbar3Action.performed += ctx => ChangeSelectedSlot(2);
            hotbar4Action.performed += ctx => ChangeSelectedSlot(3);
            hotbar5Action.performed += ctx => ChangeSelectedSlot(4);
            

        }

        public void OnDisable()
        {
            // IMPORTANT: Unsubscribe from events to prevent errors
            hotbar1Action.performed -= ctx => ChangeSelectedSlot(0);
            hotbar2Action.performed -= ctx => ChangeSelectedSlot(1);
            hotbar3Action.performed -= ctx => ChangeSelectedSlot(2);
            hotbar4Action.performed -= ctx => ChangeSelectedSlot(3);
            hotbar5Action.performed -= ctx => ChangeSelectedSlot(4);    

            hotbar1Action?.Disable();
            hotbar2Action?.Disable();
            hotbar3Action?.Disable();
            hotbar4Action?.Disable();
            hotbar5Action?.Disable();


        }
        // changes selected inventory slot on the hotbar
        public void ChangeSelectedSlot(int newValue)
        {
            
            if (selectedSlot >= 0 && selectedSlot < inventorySlots.Length)
            {
                inventorySlots[selectedSlot].Deselect();
            }

            inventorySlots[newValue].Select();
            selectedSlot = newValue;
           
            
            if (currentWeaponObject != null)
            {
                Destroy(currentWeaponObject);
                currentWeaponObject = null;
            }

            InventoryItem itemInSlot = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();

            if (weaponSwitcher != null)
            {
         
                weaponSwitcher.SelectWeapon(selectedSlot);
            }
           
        }


        // adds items to inventory
        public bool AddItems(Item item)
        {

            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventoryItem inventorySlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
                if (inventorySlot != null && inventorySlot.item == item && inventorySlot.count < maxItemCount && inventorySlot.item.stackable == true)
                {
                    if (inventorySlot.count < maxItemCount) {
                        inventorySlot.count++;

                    }
                        

                    inventorySlot.RefreshCount();
                    return true;
                }
            }


            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventorySlot slot = inventorySlots[i];
                InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();
                if (inventoryItem == null)
                {
                    SpawnNewItem(item, slot);
                    return true;
                }
            }
            return false;

        }

        // spawns new item into inventory slot
        public void SpawnNewItem(Item item, InventorySlot slot)
        {
            GameObject newItem = Instantiate(inventoryPrefab, slot.transform);
            InventoryItem items = newItem.GetComponent<InventoryItem>();
            
            items.InitialiseItem(item);
        }

        // gets selected item from inventory
        public Item GetSelectedItem(bool use)
        {
            InventorySlot slot = inventorySlots[selectedSlot];
            InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
            if (item != null)
            {
                Item newItem = item.item;
                if (use == true)
                {
                    item.count--;
                    if (item.count <= 0)
                    {
                        Destroy(item.gameObject);
                    }
                    else
                    {
                        item.RefreshCount();
                    }
                }

            }
            return null;


        }

        
    }
}
