using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformers
{
    public class WeaponController : MonoBehaviour
    {
        public GameObject Sword; 

        // --- Action Flags ---
        public bool CanAttack = true;
        public bool CanDefend = true;
        public bool CanMine = true; 

        // --- Cooldowns ---
        public float AttackCooldown = 1f;
        public float MineCooldown = 1.2f; 

        // --- Input Actions ---
        public InputActionAsset weaponActions;
        private InputAction attackAction;
        private InputAction defendAction;
        private InputAction mineAction; 

        // --- State Booleans ---
        public bool isAttacking = false;
        public bool isDefending = false;
        public bool isMining = false; 

        public void Awake()
        {
            // Find all the actions from the Input Action Asset
            var playerActionMap = weaponActions.FindActionMap("Player");
            attackAction = playerActionMap.FindAction("Attack");
            defendAction = playerActionMap.FindAction("Defend");
            mineAction = playerActionMap.FindAction("Mine"); 
        }

        // Enable and Disable Input Actions
        private void OnEnable()
        {
            attackAction.Enable();
            defendAction.Enable();
            mineAction.Enable(); // NEW: Enable the mine action
        }

        private void OnDisable()
        {
            attackAction.Disable();
            defendAction.Disable();
            mineAction.Disable(); // NEW: Disable the mine action
        }

        // attacks and defends based on player input and bools for cooldowns and triggered states
        private void Update()
        {
            
            if (attackAction.triggered && CanAttack)
            {
                SwordAttack();
            }

            if (defendAction.triggered && CanDefend)
            {
                SwordDefend();
            }

            
            if (mineAction.triggered && CanMine)
            {
                MineAction();
            }
        }

        // --- Sword Attack Logic ---
        private void SwordAttack()
        {
            if (GameManager.isInventoryOpen || GameManager.IsShopOpen) return;
            if (GameManager.enables) return; // Uncomment if you have this manager
            if (CanAttack)
            {
                isAttacking = true;
                CanAttack = false;
                Animator anim = Sword.GetComponent<Animator>();
                anim.SetTrigger("Attack");
                StartCoroutine(ResetAttack());
            }
        }

        // Resets the ability to attack after cooldown
        private IEnumerator ResetAttack()
        {
            StartCoroutine(ResetAttackBool());
            yield return new WaitForSeconds(AttackCooldown);
            CanAttack = true;
        }

        // Resets the isAttacking flag after the attack animation is likely finished
        IEnumerator ResetAttackBool()
        {
            yield return new WaitForSeconds(AttackCooldown);
            isAttacking = false;
        }

        // --- Sword Defend Logic ---
        private void SwordDefend()
        {
            if (GameManager.isInventoryOpen || GameManager.IsShopOpen) return;
            if (GameManager.enables) return; // Uncomment if you have this manager
            if (CanDefend)
            {
                isDefending = true;
                CanDefend = false;
                Animator anim = Sword.GetComponent<Animator>();
                anim.SetTrigger("Defend");
                StartCoroutine(ResetDefend());
            }
        }

        // Resets the ability to defend after cooldown
        private IEnumerator ResetDefend()
        {
            StartCoroutine(ResetDefendBool());
            yield return new WaitForSeconds(0.5f);
            CanDefend = true;
        }

        // Resets the isDefending flag after the defend animation is likely finished
        IEnumerator ResetDefendBool()
        {
            yield return new WaitForSeconds(0.5f);
            isDefending = false;
        }

        // --- NEW: MINING LOGIC ---
        private void MineAction()
        {
            // These checks prevent mining while a menu is open
            if (GameManager.isInventoryOpen || GameManager.IsShopOpen) return;
            if (GameManager.enables) return; 

            if (CanMine)
            {
                isMining = true;
                CanMine = false;
                Animator anim = Sword.GetComponent<Animator>(); 
                anim.SetTrigger("Attack"); 
                StartCoroutine(ResetMine());
            }
        }

        // Resets the ability to mine after cooldown
        private IEnumerator ResetMine()
        {
            StartCoroutine(ResetMineBool());
            yield return new WaitForSeconds(MineCooldown);
            CanMine = true;
        }

        // Resets the isMining flag after the mining animation is likely finished
        IEnumerator ResetMineBool()
        {
            // This coroutine resets the isMining flag after the animation is likely finished
            yield return new WaitForSeconds(MineCooldown);
            isMining = false;
        }
    }
}