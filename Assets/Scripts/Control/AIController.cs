using System;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Tags;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Control
{
   public class AIController : MonoBehaviour
   {
      [SerializeField] private float chaseDistance = 5f;
      [SerializeField] private float suspicionTime = 5f;
      [SerializeField] private PatrolPath patrolPath;
      [SerializeField] private float waypointTolerance = 1f;
      [SerializeField] private float waypointDwellTime = 3f;
      [Range(0,1)]
      [SerializeField] private float patrolSpeedCoefficient = 0.2f;
      
      private Fighter fighter;
      private Health health;
      private Mover mover;
      private GameObject player;

      private Vector3 guardPosition; 
      private float timeSinceLastSawPlayer = Mathf.Infinity;
      private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
      private int currentWaypointIndex = 0;

      private void Awake()
      {
         fighter = GetComponent<Fighter>();
         health = GetComponent<Health>();
         mover = GetComponent<Mover>();
         player = GameObject.FindWithTag(Tag.Player.ToString());

         guardPosition = transform.position;
      }

      private void Update()
      {
         if (health.GetIsDead()) return;

         if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
         {
            AttackBehaviour();
         }
         else if (timeSinceLastSawPlayer < suspicionTime)
         {
            SuspicionBehaviour();
         }
         else
         {
            PatrolBehaviour();
         }

         UpdateTimers();
      }

      private void UpdateTimers()
      {
         timeSinceLastSawPlayer += Time.deltaTime;
         timeSinceArrivedAtWaypoint += Time.deltaTime;
      }

      private void PatrolBehaviour()
      {
         Vector3 nextPosition = guardPosition;

         if(patrolPath != null)
         {
            if(AtWaypoint())
            {
               timeSinceArrivedAtWaypoint = 0;
               CycleWaypoint();
            }
            nextPosition = GetCurrentWaypoint();
         }

         if(timeSinceArrivedAtWaypoint > waypointDwellTime)
         {
            mover.StartMoveAction(nextPosition, patrolSpeedCoefficient);
         }
      }

      private bool AtWaypoint()
      {
         float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
         return distanceToWaypoint < waypointTolerance;
      }

      private void CycleWaypoint()
      {
         currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
      }

      private Vector3 GetCurrentWaypoint()
      {
         return patrolPath.GetWaypoint(currentWaypointIndex);
      }

      private void SuspicionBehaviour()
      {
         GetComponent<ActionScheduler>().CancelCurrentAction();
      }

      private void AttackBehaviour()
      {
         timeSinceLastSawPlayer = 0f;
         fighter.Attack(player);
      }

      private bool InAttackRangeOfPlayer()
      {
         float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
         return distanceToPlayer < chaseDistance;
      }

      //*Called by Unity
      private void OnDrawGizmosSelected()
      {
         Gizmos.color = Color.blue;
         Gizmos.DrawWireSphere(transform.position, chaseDistance);
      }
   }
}
