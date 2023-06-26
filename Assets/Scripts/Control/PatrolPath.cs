using UnityEngine;

namespace RPG.Control
{   
   public class PatrolPath : MonoBehaviour
   {
      private const float WAYPOINT_GIZMO_RADIUS = .3f;

      //*Called by Unity
      private void OnDrawGizmos()
      {
         Gizmos.color = Color.gray;

         for (int waypointIndex = 0; waypointIndex< transform.childCount; waypointIndex ++)
         {
            int nextWaypointIndex = GetNextIndex(waypointIndex);
            Gizmos.DrawSphere(GetWaypoint(waypointIndex), WAYPOINT_GIZMO_RADIUS);
            Gizmos.DrawLine(GetWaypoint(waypointIndex), GetWaypoint(nextWaypointIndex));
         }
      }

      public int GetNextIndex(int waypointIndex)
      {
         return (waypointIndex + 1 < transform.childCount) ? (waypointIndex + 1) : 0;
      }

      public Vector3 GetWaypoint(int waypointIndex)
      {
         return transform.GetChild(waypointIndex).position;
      }
   }
}
