// MineableResource.cs
using UnityEngine;

namespace Platformers 
{

    public class MineableResource : MonoBehaviour
    {
        [Header("Resource Stats")]
        public float health = 50f; // How many hits it can take

        [Header("Loot Drops")]
        public GameObject lootPrefab; // The item that will spawn when destroyed
        public int lootAmount = 3;   // How many items to spawn

        [SerializeField] private FloatingHealthbar healthbar;

        private FirstPersonController player;
        public GameObject droppedItemPrefab;
        public Item item; // The item data to assign to the dropped loot

        // This is a public method that other scripts (like the player) can call.

        private void Awake()
        {
            healthbar = GetComponentInChildren<FloatingHealthbar>();
            player = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
        }
        public void TakeDamage(float damageAmount)
        {
            // Subtract the damage from our health
            health -= damageAmount;
            healthbar.UpdateHealthbar(health, 50f);

            // Check if the resource should be destroyed
            if (health <= 0)
            {
                DestroyResource();
            }
        }

        private void DestroyResource()
        {

            // Spawn loot items at the resource's position
            if (droppedItemPrefab != null)
            {
                for (int i = 0; i < lootAmount; i++)
                {

                    // drops the item a bit in front of the player

                    Vector3 dropOffset = player.transform.forward * 4.5f + player.transform.up * 2f; // 1.5 units forward, 0.5 units up


                    Vector3 targetDropPosition = player.transform.position + dropOffset;
                    
                    GameObject droppedGameObject = Instantiate(droppedItemPrefab, targetDropPosition, Quaternion.identity);

                    // Assign the item data to the dropped item object
                    ItemObjects itemObjectComponent = droppedGameObject.GetComponent<ItemObjects>();
                    // Check if the component exists before using it and assign the item data   
                    if (droppedGameObject.TryGetComponent<ItemObjects>(out _))
                    {
                        
                        itemObjectComponent.SetItem(item, targetDropPosition);
                        itemObjectComponent.position = targetDropPosition;
                        

                        if (droppedGameObject.TryGetComponent<Rigidbody>(out var rb))
                        {
                            
                            rb.AddForce(player.transform.forward * 5f, ForceMode.VelocityChange); ; // Increased force for more noticeable effect
                        }
                    }
                    else
                    {
                        Debug.LogError("Dropped Item Prefab is missing an ItemObject component!", droppedGameObject);
                    }

                }
            }

        
            Destroy(gameObject);
        }
    }


}