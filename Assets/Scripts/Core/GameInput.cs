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

      private PlayerInputActions playerInputActions;


      private void Awake()
      {
         Instance = this;

         playerInputActions = new PlayerInputActions();

         playerInputActions.Enable();
      }
   }
}