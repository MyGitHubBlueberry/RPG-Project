using System;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
   public class PlayerController : MonoBehaviour
   {
      private Fighter fighter;
      private int leftMouseButton = 0;

      private void Awake()
      {
         fighter = GetComponent<Fighter>();
      }

      private void Update()
      {
         if(HandleCombat()) return;
         if(HandleMovement()) return;
         print("nothing to do");
      }

      private bool HandleCombat()
      {
         RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
         if(ContainsCombatTarget(out CombatTarget target, hits))
         {
            if(Input.GetMouseButtonDown(leftMouseButton))
            {
               fighter.Attack(target.gameObject);
            }
            return true;
         }
         return false;
      }

      private bool HandleMovement()
      {
         bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit);
         if (hasHit)
         {
            if(Input.GetMouseButton(leftMouseButton))
            {
               GetComponent<Mover>().StartMoving(hit.point);
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
            if(!fighter.CanAttack(target.gameObject)) continue;
            
            return true;
         }
         target = null;
         return false;
      }

      private Ray GetMouseRay()
      {
         return Camera.main.ScreenPointToRay(Input.mousePosition);
      }
   }
}
