using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Platformers
{
    public class SwordCollisionDetector : MonoBehaviour
    {
        

        [Header("Combat Stats")]
        public int attackDamage = 10;

        [Header("Mining Stats")]
        public int miningDamage = 5; // NEW: Damage dealt to resources

        // References
        public WeaponController weaponController;
        private CharacterStatss characterStats;
        [SerializeField] private MineableResource mineable;

        public void Start()
        {
            // Find and assign CharacterStatss component from the "Characters" GameObject
            GameObject charactersGO = GameObject.Find("Characters");
            if (charactersGO != null)
            {
                characterStats = charactersGO.GetComponent<CharacterStatss>();
            }
        }
        // This method is called when another collider enters the trigger collider attached to the sword.
        private void OnTriggerEnter(Collider other)
        {
            
            if (other.CompareTag("Mineable"))
            {

                if (other.TryGetComponent<MineableResource>(out var resource))
                {
                    
                    resource.TakeDamage(miningDamage);
                }
            }
            // Check if the collided object is an enemy and if the sword is currently in an attacking state.

            else if (other.CompareTag("Enemy") && weaponController.isAttacking)
            {
                Debug.Log("Sword hit an enemy: " + other.name);

                if (other.TryGetComponent<EnemyAI>(out var enemy))
                {
                    // Tell the ENEMY to take damage.
                    int totalDamage = attackDamage + (characterStats != null ? characterStats.BaseStrength : 0);
                    enemy.TakeDamage(totalDamage);
                }
            }

            
        }
    }
}

