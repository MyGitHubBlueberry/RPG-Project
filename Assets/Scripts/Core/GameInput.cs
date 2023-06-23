using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

namespace RPG.Core
{
   public class GameInput : MonoBehaviour
   {
      public static GameInput Instance;

      public event EventHandler OnLeftClickPerformed;

      private PlayerInputActions playerInputActions;


      private void Awake()
      {
         Instance = this;

         playerInputActions = new PlayerInputActions();

         playerInputActions.Enable();

         playerInputActions.Mouse.LeftClick.performed += LeftClick_performed;
      }

      private void LeftClick_performed(CallbackContext context)
      {
         OnLeftClickPerformed?.Invoke(this, EventArgs.Empty);
      }

      public bool IsLeftMouseButtonPressed()
      {
         return playerInputActions.Mouse.LeftClick.inProgress;
      }
   }
}