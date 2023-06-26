using UnityEngine;

namespace RPG.Control
{   
   public class PatrolPath : MonoBehaviour
   {
      private const float WAYPOINT_GIZMO_RADIUS = .3f;
      private void OnDrawGizmos()
      {
         Gizmos.color = Color.gray;

         for (int waypointIndex = 0; waypointIndex< transform.childCount; waypointIndex ++)
         {  
            int nextWaypointIndex = waypointIndex + 1; 
            Transform currentWaypoint = transform.GetChild(waypointIndex);
            Gizmos.DrawSphere(currentWaypoint.transform.position, WAYPOINT_GIZMO_RADIUS);
            if(nextWaypointIndex == transform.childCount) nextWaypointIndex = 0;
            Gizmos.DrawLine(currentWaypoint.position, transform.GetChild(nextWaypointIndex).position);
         }
      }     
   }
}
