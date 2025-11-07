using Controller;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; // Not explicitly used, can remove if not needed
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

namespace Platformers
{
    public class FirstPersonController : MonoBehaviour
    {
        private CharacterController characterController;
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float mouseSensitivity = 2f;
        [SerializeField] private float verticalRotationLimit = 80f;
        [SerializeField] private float sprintMultiplier = 2f;
        [SerializeField] private float jumpHeight = 5.0f;
        [SerializeField] private float gravity = 9.81f;
        [SerializeField] private InputActionAsset playerActions; 
        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction sprintAction;
        private InputAction lookAction;
        private InputAction mineAction;
        private Vector2 moveInput;
        private Vector2 lookInput;
        private GameObject inventoryMenu;
        private InputAction attackAction;
        private InputAction defendAction;
        private InputAction interaction;
        private InventoryManager inventoryManager;
        private HealthManager healthManager;
        private readonly Animator animator;
        [SerializeField] private ShopManager shopManager;
        [HideInInspector] public static bool isInventoryOpen = false;
        public float mineDistance = 3f; // How far the player can mine
        public LayerMask mineableLayer;         

        private bool canMine = true; // To prevent spamming mining
        public float mineRate = 1f; // How often the player can mine (seconds between mines)
        public Button closeButton;



        private float verticalRotation;
        private Camera mainCamera;
        private Vector3 currentMovement = Vector3.zero;

        public float attackDamage = 1f;
        public float attackDistance = 2f;
        public float attackRate = 1f;
        public float attackDelay = 1f;
        public WeaponController weaponController;

        public LayerMask npcLayer;

        public LayerMask attackLayer;
        public GameObject hitEffect;    

        

        public float interactionDistance = 4f;


        public const string ANIMATION_ATTACK_01 = "Attack_01";
        public const string ANIMATION_ATTACK_02 = "Attack_02";

       
        
        public bool controlsEnabled = true; 

        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inventoryMenu = GameObject.Find("MainInvenGroup");
            inventoryMenu.SetActive(false);

            inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
         
            shopManager.HideCoinsText();
            shopManager.gameObject.SetActive(false);
            shopManager.CloseShop();

            closeButton.gameObject.SetActive(false);


            characterController = GetComponent<CharacterController>();
            healthManager = GameObject.Find("HealthManager").GetComponent<HealthManager>();

            
            mainCamera = Camera.main; 

            // Initial cursor state for gameplay
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Ensure playerActions is assigned in the Inspector
            if (playerActions == null)
            {
                Debug.LogError("Player Actions Asset not assigned in FirstPersonController!", this);
                return;
            }
            
            moveAction = playerActions.FindActionMap("Player").FindAction("Move");
            jumpAction = playerActions.FindActionMap("Player").FindAction("Jump");
            sprintAction = playerActions.FindActionMap("Player").FindAction("Sprint");
            lookAction = playerActions.FindActionMap("Player").FindAction("Look");
            attackAction = playerActions.FindActionMap("Player").FindAction("Attack");
            defendAction = playerActions.FindActionMap("Player").FindAction("Defend");
            mineAction = playerActions.FindActionMap("Player").FindAction("Mine");
            interaction = playerActions.FindActionMap("Player").FindAction("Interact");

            // Input action callbacks
            moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            moveAction.canceled += ctx => moveInput = Vector2.zero;
            
            lookAction.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
            lookAction.canceled += ctx => lookInput = Vector2.zero;

            
        }
        // Mining method based on raycasting from the camera
        public void Mine()
        {
            if (!canMine || GameManager.isInventoryOpen) return; // Don't mine if not ready or inventory is open

            
            
            canMine = false;
            Invoke(nameof(ResetMine), mineRate); 

            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, mineDistance, mineableLayer))
            {
                Debug.Log("Hit something mineable: " + hit.collider.name);



                // checks if the hit object has a MineableResource component
                if (hit.collider.TryGetComponent<MineableResource>(out var mineable))
                {
                    mineable.TakeDamage(attackDamage); // Assuming mining does "damage" to the resource
                }
                
            }
            
        }

        // Resets the mining ability after cooldown
        private void ResetMine()
        {
            canMine = true;
        }


       
        // Handles right-click interaction with NPCs
        private void HandleRightClickInteract()
        {
            if (GameManager.isInventoryOpen || !controlsEnabled)
            {
                Debug.Log("Cannot interact right now.");
                // Don't interact if inventory is open, game is paused, or controls are disabled
                return;
            }

            // Perform a raycast from the center of the screen
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            // Check if the ray hits an enemy object within a certain distance
            if (Physics.Raycast(ray, out hit, attackDistance, npcLayer)) // Reuse attackDistance for interaction range
            {
                Debug.Log("Interacted with: " + hit.collider.name);
                // Check if the hit object has a NormalAI component
                NormalAI enemyHealth = hit.collider.GetComponent<NormalAI>();
                if (enemyHealth != null)
                {
                    Debug.Log("Interacted with NPC: " + enemyHealth);
                    shopManager.gameObject.SetActive(true);
                    shopManager.ShowCoinsText();
                    shopManager.OpenShop();
                    closeButton.gameObject.SetActive(true);


                }
                
            }
            
        }

        // Handles all the movement and rotation each frame
        void Update()
        {

            if (characterController == null)
            {
                Debug.LogError("The FirstPersonController script on the object '" + gameObject.name + "' is missing a CharacterController component.", this.gameObject);
            }
            HandleMovement();
            HandleRotation();
            InventoryState();
        }


       
        // Enable and disable input actions 
        private void OnEnable()
        {
            
            if (controlsEnabled)
            {
                EnableInputActions();
            }
        }
        private void OnDisable()
        {
            DisableInputActions();
        }

        // Enable and disable all input actions
        private void EnableInputActions()
        {
            moveAction.Enable();
            jumpAction.Enable();
            sprintAction.Enable();
            lookAction.Enable();

            mineAction.Enable();
            interaction.Enable();
            interaction.performed += ctx => HandleRightClickInteract();
        }

        private void DisableInputActions()
        {
            moveAction.Disable();
            jumpAction.Disable();
            sprintAction.Disable();
            lookAction.Disable();

            mineAction.Disable();
            interaction.Disable();
            interaction.performed -= ctx => HandleRightClickInteract();
        }
        

        // Handle collisions with projectiles so that the player can take damage or block them
        private void OnCollisionEnter(Collision collision)
        {
            // We only care about objects tagged "Projectile".
            if (collision.gameObject.CompareTag("Projectile"))
            {

                Debug.Log("hit");
                // --- THIS IS THE NEW LOGIC ---
               
                if (weaponController != null && weaponController.isDefending)
                {
                    // --- DEFENDING LOGIC ---
                    Debug.Log("BLOCKED! Projectile was deflected.");


                }
                else
                {
                    // --- NOT DEFENDING LOGIC (Take Damage) ---
                    Debug.Log("HIT! Player took damage from a projectile.");
                    Debug.Log(healthManager.health);
                    Debug.Log(healthManager.maxHealth);
                    // Try to get the Projectile component to access damage value.

                    if (collision.gameObject.TryGetComponent<Projectile>(out var projectile))
                    {

                        if (healthManager != null)
                        {
                            healthManager.TakeDamage(projectile.damage);

                        }
                    }
                }


            }
            
        }

        // Handles gravity and jumping mechanics
        private void HandleGravityAndJumping()
        {
            if (characterController.isGrounded)
            {
                currentMovement.y = -0.5f; 
                if (jumpAction.triggered)
                { 
                    currentMovement.y = jumpHeight;
                }
            }
            else
            {
                currentMovement.y -= gravity * Time.deltaTime;
            }
        }

        private void HandleMovement()
        {
            // Read sprint input
            float speedModifier = sprintAction.ReadValue<float>() > 0 ? sprintMultiplier : 1f;

            // Calculate movement direction relative to player's forward
            Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
            moveDirection.Normalize(); // Normalize to prevent faster diagonal movement

            // Apply speed and modifier
            currentMovement.x = moveDirection.x * movementSpeed * speedModifier;
            currentMovement.z = moveDirection.z * movementSpeed * speedModifier;

            HandleGravityAndJumping(); // Apply gravity and jump to currentMovement.y

            characterController.Move(currentMovement * Time.deltaTime);
        }


        // This method is called when the CharacterController hits a collider while moving

        private void OnControllerColliderHit(ControllerColliderHit hit) // 
        {
            
            if (hit.gameObject.CompareTag("DroppedItem"))
            {
                Debug.Log("Collided with DroppedItem"); 
                if (hit.gameObject.TryGetComponent<ItemObjects>(out var itemObject))
                {
                    Debug.Log("Picking up item");
                

                    inventoryManager.AddItems(itemObject.item);

                    // Add item to player's inventory
                    Destroy(hit.gameObject);
                }
            }
            
        }



        // Manages the inventory menu state and input
        public void InventoryState()
        {

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                // Prevent opening inventory if shop is open
                if (GameManager.IsShopOpen) return;
               
                if (inventoryMenu.activeSelf)
                {
                    inventoryMenu.SetActive(false);
                    SetControlsEnabled(true); // Re-enable controls when closing inventory
                    isInventoryOpen = false;
                    GameManager.isInventoryOpen = false;
                }
                else
                {
                    inventoryMenu.SetActive(true);
                    SetControlsEnabled(false); // Disable controls when opening inventory
                    isInventoryOpen = true;
                    GameManager.isInventoryOpen = true;
                }
            }

            

        }

        private void HandleRotation()
        {
            // Apply horizontal rotation to the player body (around Y-axis)
            float mouseX = lookInput.x * mouseSensitivity;
            transform.Rotate(Vector3.up * mouseX);

            // Apply vertical rotation to the camera (around X-axis)
            float mouseY = lookInput.y * mouseSensitivity;
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
            mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }


        // Enable or disable player controls normally when opening UI menus
        public void SetControlsEnabled(bool enabled = true)
        {

            controlsEnabled = enabled;

            if (enabled)
            {
                
                playerActions.Enable();

                // 2. Lock and hide the cursor for first-person gameplay.
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                
            }
            else
            {
                
                playerActions.Disable();

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                moveInput = Vector2.zero;
                lookInput = Vector2.zero;
                
            }
        }
    }
}