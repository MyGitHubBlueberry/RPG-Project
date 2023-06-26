using System;
using RPG.Combat;
using UnityEngine;

namespace RPG.Core
{   
   public class ActionScheduler : MonoBehaviour
   {
      private IAction currentAction;

      private void Awake()
      {
         GetComponent<Health>().OnZeroHealth += Health_OnZeroHealth;
      }

      private void Health_OnZeroHealth(object sender, EventArgs e)
      {
         CancelCurrentAction();
      }

      public void StartAction(IAction action)
      {
         if(currentAction == action) return;
         if(currentAction != null)
         {
            currentAction.Cancel();
         }
         currentAction = action;
      }

      public void CancelCurrentAction()
      {
         StartAction(null);
      }
   }
}