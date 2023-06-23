using System;
using UnityEngine;
using RPG.Core;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
   public class PlayerController : MonoBehaviour
   {
      private bool isMovingToCursor;

      private void Start()
      {
         GameInput.Instance.OnLeftClickPerformed += GameInput_OnLeftClickPerformed;
         GameInput.Instance.OnLeftClickStarted += GameInput_OnLeftClickStarted;
         GameInput.Instance.OnLeftClickCanceled += GameInput_OnLeftClickCanceled;
      }

      #region EventMethods
      private void GameInput_OnLeftClickCanceled(object sender, EventArgs e)
      {
         isMovingToCursor = false;
      }

      private void GameInput_OnLeftClickStarted(object sender, EventArgs e)
      {
         isMovingToCursor = true;
      }

      private void GameInput_OnLeftClickPerformed(object sender, EventArgs e)
      {
         HandleCombat();
         MoveToCursor();
      }
      #endregion

      private void Update()
      {
         HandleMovement();
      }

      private void HandleCombat()
      {
         RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

         foreach (RaycastHit hit in hits)
         {
            CombatTarget target = hit.transform.GetComponent<CombatTarget>();
            if(target == null) continue;

            GetComponent<Fighter>().Attack(target);
               
            return;
         }
      }

      private void HandleMovement()
      {
         if(isMovingToCursor)
         {
            MoveToCursor();
            return;
         }
      }

      private void MoveToCursor()
      {
         bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit raycastHit);

         if (hasHit)
         {
            GetComponent<Mover>().MoveTo(raycastHit.point);
         }
      }

      private static Ray GetMouseRay()
      {
         return Camera.main.ScreenPointToRay(Input.mousePosition);
      }
   }
}
