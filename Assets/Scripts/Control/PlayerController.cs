using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System.Linq;
using UnityEngine.EventSystems;
using System;
using UnityEngine.AI;

namespace RPG.Control
{
   public partial class PlayerController : MonoBehaviour
   {
      [SerializeField] private float maxNavMeshProjectionDistance = 1f;
      [SerializeField] private CursorMapping[] cursorMappings;
      private Fighter fighter;
      private Health health;
      private CombatTarget combatTarget;

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

      private bool InteractWithUI()
      {
         if(EventSystem.current.IsPointerOverGameObject())
         {
            SetCursor(CursorType.UI);
            return true;
         }
         return false;
      }

      private bool InteractWithComponent()
      {
         RaycastHit[] hits = RaycastAllSorted();
         foreach (RaycastHit hit in hits)
         {
            IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
            foreach(IRaycastable raycastable in raycastables)
            {
               if(raycastable.HandleRaycast(this))
               {
                  SetCursor(raycastable.GetCursorType());
                  return true;
               }   
            }
         }
         return false;
      }

      private RaycastHit[] RaycastAllSorted()
      {
         RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
         float[] distances = new float[hits.Length];
         for (int i = 0; i < distances.Length; i++)
         {
            distances[i] = hits[i].distance;
         }
         Array.Sort(distances, hits);
         return hits;
      }

      private bool InteractWitMovement()
      {
         if (RaycastNavMesh(out Vector3 target))
         {
            if(Input.GetMouseButton(0))
            {
               GetComponent<Mover>().StartMoveAction(target, 1f);
            }
            SetCursor(CursorType.Movement);
            return true;
         }
         return false;
      }

      private bool RaycastNavMesh(out Vector3 target)
      {
         target = new Vector3();

         if (!Physics.Raycast(GetMouseRay(), out RaycastHit hit) ||
             !NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit,
             maxNavMeshProjectionDistance, NavMesh.AllAreas))
         return false;

         target = navMeshHit.position;
         return true;
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
