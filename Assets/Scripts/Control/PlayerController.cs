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
         HandleCombat();
      }
      #endregion

      private void Update()
      {
         if(HandleMovement()) return;
         print("nothing to do");
      }

      private void HandleCombat()
      {
         RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

         if(ContainsCombatTarget(out CombatTarget target, hits))
         {
            GetComponent<Fighter>().Attack(target);
         }
      }

      private bool HandleMovement()
      {
         bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit);

         if (hasHit)
         {
            if(GameInput.Instance.IsLeftMouseButtonPressed() && !ContainsCombatTarget(hit))
            {
               GetComponent<Mover>().MoveTo(hit.point);
            }
            return true;
         }
         return false;
      }

      private bool ContainsCombatTarget(out CombatTarget target, params RaycastHit[] hits)
      {
         foreach (RaycastHit hit in hits)
         {
            target = hit.transform.GetComponent<CombatTarget>();
            if(target == null) continue;
            
            return true;
         }
         target = null;
         return false;
      }

      private bool ContainsCombatTarget(params RaycastHit[] hits)
      {
         return ContainsCombatTarget(out _, hits);
      }

      private static Ray GetMouseRay()
      {
         return Camera.main.ScreenPointToRay(Input.mousePosition);
      }
   }
}
