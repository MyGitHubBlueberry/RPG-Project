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

      public event EventHandler OnMoveToCursorAction;
      public event EventHandler OnMoveToCursorStarted;
      public event EventHandler OnMoveToCursorCanceled;
      public event EventHandler OnAttackPerformed;

      private PlayerInputActions playerInputActions;


      private void Awake()
      {
         Instance = this;

         playerInputActions = new PlayerInputActions();

         playerInputActions.Enable();

         playerInputActions.Movement.MoveToCursor.performed += MoveToCursor_performed;
         playerInputActions.Movement.MoveToCursor.started += MoveToCursor_started;
         playerInputActions.Movement.MoveToCursor.canceled += MoveToCursor_canceled;

         playerInputActions.Combat.Attack.performed += Attack_performed;
      }

      private void Attack_performed(CallbackContext context)
      {
         OnAttackPerformed?.Invoke(this, EventArgs.Empty);
      }

      private void MoveToCursor_canceled(CallbackContext context)
      {
         OnMoveToCursorCanceled?.Invoke(this, EventArgs.Empty);
      }

      private void MoveToCursor_started(CallbackContext context)
      {
         OnMoveToCursorStarted?.Invoke(this, EventArgs.Empty);
      }

      private void MoveToCursor_performed(CallbackContext context)
      {
         OnMoveToCursorAction?.Invoke(this, EventArgs.Empty);
      }
   }
}