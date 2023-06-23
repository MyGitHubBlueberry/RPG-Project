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
         GameInput.Instance.OnMoveToCursorAction += GameInput_OnMoveToCursorAction;
         GameInput.Instance.OnMoveToCursorStarted += GameInput_OnMoveToCursorStarted;
         GameInput.Instance.OnMoveToCursorCanceled += GameInput_OnMoveToCursorCanceled;
         GameInput.Instance.OnAttackPerformed += GameInput_OnAttackPerformed;
      }

      #region EventMethods
      private void GameInput_OnMoveToCursorCanceled(object sender, EventArgs e)
      {
         isMovingToCursor = false;
      }

      private void GameInput_OnMoveToCursorStarted(object sender, EventArgs e)
      {
         isMovingToCursor = true;
      }

      private void GameInput_OnMoveToCursorAction(object sender, EventArgs e)
      {
         MoveToCursor();
      }

      private void GameInput_OnAttackPerformed(object sender, EventArgs e)
      {
         HandleCombat();
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
