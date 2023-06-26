using System;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
   public class PlayerController : MonoBehaviour
   {
      private Fighter fighter;
      private Health health;
      private int leftMouseButton = 0;

      private void Awake()
      {
         fighter = GetComponent<Fighter>();
         health = GetComponent<Health>();
      }

      private void Update()
      {
         if(health.GetIsDead()) return;

         if(HandleCombat()) return;
         if(HandleMovement()) return;
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
               GetComponent<Mover>().StartMoveAction(hit.point);
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
