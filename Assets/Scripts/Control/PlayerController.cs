using System;
using UnityEngine;
using RPG.Core;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
   public class PlayerController : MonoBehaviour
   {

      private void Start()
      {
         GameInput.Instance.OnLeftClickPerformed += GameInput_OnLeftClickPerformed;
      }

      #region EventMethods
      private void GameInput_OnLeftClickPerformed(object sender, EventArgs e)
      {
         if(TryHandleCombat()) return;
         MoveToCursor();
      }
      #endregion

      private void Update()
      {
         HandleMovement();
      }

      private bool TryHandleCombat()
      {
         RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

         foreach (RaycastHit hit in hits)
         {
            CombatTarget target = hit.transform.GetComponent<CombatTarget>();
            if(target == null) continue;

            GetComponent<Fighter>().Attack(target);
               
            return true;
         }

         return false;
      }

      private void HandleMovement()
      {
         if(GameInput.Instance.GetLeftMouseButtonPressed())
         {
            MoveToCursor();
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
