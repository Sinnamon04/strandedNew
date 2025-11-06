
using UnityEngine;

namespace Platformers
{

    public class WeaponSwitching : MonoBehaviour
    {
        
        public InventorySlot[] toolbar;
        
        public Item[] items;

        public GameObject itemHolder;

        void Awake()
        {
            GameObject toolbarObject = GameObject.Find("Toolbar");
           
        }
        // A public method to select a weapon by its index.
        public void SelectWeapon(int weaponIndex)
        {
            
            if (weaponIndex < 0 || weaponIndex >= transform.childCount)
            {
                
                UnequipAll();
                return;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                
                Transform weapon = transform.GetChild(i);

                weapon.gameObject.SetActive(i == weaponIndex);
            }

        }

        // A public method to hide all weapons.
        public void UnequipAll()
        {
            foreach (Transform weapon in transform)
            {
                weapon.gameObject.SetActive(false);
            }

        }

        // Start is called before the first frame update and unequips all weapons.
        void Start()
        {
            UnequipAll();
        }
    }
}