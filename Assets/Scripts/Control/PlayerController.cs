using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;
using System.Linq;
using UnityEngine.EventSystems;

namespace RPG.Control
{
   public class PlayerController : MonoBehaviour
   {
      [SerializeField] private CursorMapping[] cursorMappings;
      private Fighter fighter;
      private Health health;
      private CombatTarget combatTarget;

      enum CursorType
      {
         None,
         UI,
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
         combatTarget = GetComponent<CombatTarget>();
         fighter = GetComponent<Fighter>();
         health = GetComponent<Health>();
      }

      private void Update()
      {
         if(InteractWithUI()) return;
         if(health.IsDead()) 
         {
            SetCursor(CursorType.None);
            return;
         }

         if(InteractWithComponent()) return;
         if(InteractWitMovement()) return;

         SetCursor(CursorType.None);
      }

      private bool InteractWithComponent()
      {
         RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
         foreach (RaycastHit hit in hits)
         {
            IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
            foreach(IRaycastable raycastable in raycastables)
            {
               if(raycastable.HandleRaycast(this))
               {
                  SetCursor(CursorType.Combat);
                  return true;
               }   
            }
         }
         return false;
      }

      private bool InteractWithUI()
      {
         if(EventSystem.current.IsPointerOverGameObject())
         {
            SetCursor(CursorType.UI);
            return true;
         }
         return false;
      }

      private bool InteractWitMovement()
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
