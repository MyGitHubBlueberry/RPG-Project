using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;
using System.Linq;

namespace RPG.Control
{
   public class PlayerController : MonoBehaviour
   {
      [SerializeField] private CursorMapping[] cursorMappings;
      private Fighter fighter;
      private Health health;

      enum CursorType
      {
         None,
         Movement,
         Combat,
      }

      [System.Serializable]
      struct CursorMapping
      {
         [SerializeField] private string name;
         public CursorType type;
         public Texture2D texture;
         public Vector2 hotspot;
      }

      private void Awake()
      {
         fighter = GetComponent<Fighter>();
         health = GetComponent<Health>();
      }

      private void Update()
      {
         if(health.IsDead()) return;

         if(HandleCombat()) return;
         if(HandleMovement()) return;

         SetCursor(CursorType.None);
      }

      private bool HandleCombat()
      {
         RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
         if(ContainsCombatTarget(out CombatTarget target, hits))
         {
            if(Input.GetMouseButton(0))
            {
               fighter.Attack(target.gameObject);
            }
            SetCursor(CursorType.Combat);
            return true;
         }
         return false;
      }

      private bool HandleMovement()
      {
         bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit);
         if (hasHit)
         {
            if(Input.GetMouseButton(0))
            {
               GetComponent<Mover>().StartMoveAction(hit.point, 1f);
            }
            SetCursor(CursorType.Movement);
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

      private void SetCursor(CursorType type)
      {
         CursorMapping mapping = GetCursorMapping(type);
         Cursor.SetCursor(mapping.texture, mapping.hotspot,CursorMode.Auto);
      }

      private CursorMapping GetCursorMapping(CursorType type)
      {
         return cursorMappings.Where(mapping => mapping.type == type).FirstOrDefault();
      }

      private Ray GetMouseRay()
      {
         return Camera.main.ScreenPointToRay(Input.mousePosition);
      }
   }
}
