using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Platformers
{
    public class InventoryItem : MonoBehaviour,IBeginDragHandler, IEndDragHandler,IDragHandler,IPointerClickHandler
    {


        [Header("UI")]
        public Image image;
        public TextMeshProUGUI countText;
        public InventoryManager inventoryManager;


        [HideInInspector]public Transform parentAfterDrag;
        [HideInInspector] public int count = 1;
        [HideInInspector] public Item item;
        public InputAction mouse;
        public GameObject healths;
        public GameObject hungers;
        private HealthManager healthManager;
        private StaminaManager staminaManager;
        private GameObject player;
        public GameObject droppedItemPrefab;
        private CharacterStatss characterStatss;

        // enables and disables input actions
        public void OnEnable()
        {
         
            mouse.Enable();
        
        }
        public void OnDisable()
        {
         
            mouse.Disable();

        }

        public void Awake()
        {
            healths = GameObject.Find("HealthManager");
            hungers = GameObject.Find("StaminaManager");
            healthManager = healths.GetComponent<HealthManager>();
            staminaManager = hungers.GetComponent<StaminaManager>();
            player = GameObject.Find("FPSController");
            characterStatss = GameObject.Find("Characters").GetComponent<CharacterStatss>();

        }

        // initializes item into inventory item
        public void InitialiseItem(Item newItem)
        {
            item = newItem;
            image.sprite = newItem.image;
            image.color = new Color(9, 95, 154, 400);
            RefreshCount();


        }
        // updates the count display
        public void RefreshCount()
        {
            countText.text = count.ToString();
            bool isActive = count > 1;
            countText.gameObject.SetActive(isActive);
        }

        // this section is the drag and drop functionality
        public void OnBeginDrag(PointerEventData eventData)
        {

            parentAfterDrag = transform.parent;
            
            EquipmentSlot startingSlot = parentAfterDrag.GetComponent<EquipmentSlot>();

            if (startingSlot != null)
            {
   
                startingSlot.UpdateEquipmentStats();
            }

       
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;

            transform.SetParent(parentAfterDrag);

            
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {

            if (eventData.button == PointerEventData.InputButton.Right)
            {

                if (FirstPersonController.isInventoryOpen) {
                    UseSelectedItem();
                }
                
                

            }
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {

                if (FirstPersonController.isInventoryOpen) {
                    DropItem();
                }
            }

        
        }
        

        public void DropItem(bool surviveSceneLoad = false)
        {
   
                Vector3 dropOffset = player.transform.forward * 4.5f + player.transform.up * 2f; // 1.5 units forward, 0.5 units up

                // Calculate the final drop position
                Vector3 targetDropPosition = player.transform.position + dropOffset;
               
            // Instantiate the dropped item (which should have the ItemObject script attached)
                GameObject droppedGameObject = Instantiate(droppedItemPrefab, targetDropPosition, Quaternion.identity);
           
                // Get the ItemObject component from the instantiated GameObject
                ItemObjects itemObjectComponent = droppedGameObject.GetComponent<ItemObjects>();

              

                // Check if ItemObject component exists, which it should if the prefab is set up correctly
                if (droppedGameObject.TryGetComponent<ItemObjects>(out var itemObject))
                    {
                        // Pass the item data from this InventoryItem to the new ItemObject
                        itemObjectComponent.SetItem(item,targetDropPosition);
                        itemObjectComponent.position = targetDropPosition;
                    // Use the new SetItem method

                    if (droppedGameObject.TryGetComponent<Rigidbody>(out var rb))
                    {
                        // Add force relative to player's forward direction
                       rb.AddForce(player.transform.forward * 5f, ForceMode.VelocityChange);; // Increased force for more noticeable effect
                    }
                }
                
                count--;
                if (count <= 0)
                {
                    Destroy(gameObject); // Destroy the UI inventory item slot if empty
                }
                else
                {
                    RefreshCount();
                }
            
        }
       
        public void UseSelectedItem()
        {

            InventorySlot slot = transform.parent.GetComponent<InventorySlot>();
            InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
            if (item != null)
            {
                Item newItem = item.item;


                if (newItem.actionType == ActionType.Consumable) 
                {
                    Debug.Log(healthManager);

                    healthManager.Heal(newItem.healthBonus);
                    staminaManager.ChangeHunger(newItem.staminaBonus);
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
           
        }



    }
}
