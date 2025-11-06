using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Platformers
{
    [CreateAssetMenu(fileName = "InputReader",menuName = "Platformer/InputReaderActions")]
    public class InputReader : ScriptableObject, InputReaders.IPlayerActions
    {
        
        public event UnityAction<Vector2> MoveEvent = delegate { };
        public event UnityAction JumpEvent = delegate { };
        public event UnityAction<Vector2,bool> LookEvent = delegate { };
        public event UnityAction EnableMouseControlCamera = delegate { };
        public event UnityAction DisableMouseControlCamera = delegate { };

        InputReaders playerActions;

        public Vector3 Direction => (Vector3)playerActions.Player.Move.ReadValue<Vector2>();

        private void OnEnable()
        {
            if (playerActions == null)
            {
                playerActions = new InputReaders();
                playerActions.Player.SetCallbacks(this);
            }
            playerActions.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookEvent.Invoke(context.ReadValue<Vector2>(), context.control.device.name == "Mouse");
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnMouseControlCamera(InputAction.CallbackContext context)
        {
            switch (context.phase) { 
            
                case InputActionPhase.Started:
                    EnableMouseControlCamera.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    DisableMouseControlCamera.Invoke();
                    break;
            }
        }
    }
}
