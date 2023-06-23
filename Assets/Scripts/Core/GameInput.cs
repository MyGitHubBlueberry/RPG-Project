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
      public event EventHandler OnLeftClickStarted;
      public event EventHandler OnLeftClickCanceled;

      private PlayerInputActions playerInputActions;


      private void Awake()
      {
         Instance = this;

         playerInputActions = new PlayerInputActions();

         playerInputActions.Enable();

         playerInputActions.Mouse.LeftClick.performed += LeftClick_performed;
         playerInputActions.Mouse.LeftClick.started += LeftClick_started;
         playerInputActions.Mouse.LeftClick.canceled += LeftClick_canceled;
      }

      private void LeftClick_canceled(CallbackContext context)
      {
         OnLeftClickCanceled?.Invoke(this, EventArgs.Empty);
      }

      private void LeftClick_started(CallbackContext context)
      {
         OnLeftClickStarted?.Invoke(this, EventArgs.Empty);
      }

      private void LeftClick_performed(CallbackContext context)
      {
         OnLeftClickPerformed?.Invoke(this, EventArgs.Empty);
      }
   }
}