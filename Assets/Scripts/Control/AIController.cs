using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
   public class AIController : MonoBehaviour
   {
      [SerializeField] private float chaseDistance = 5f;
      [SerializeField] private float suspicionTime = 5f;
      [SerializeField] private float waypointTolerance = 1f;
      [SerializeField] private PatrolPath patrolPath;
      
      private const string PLAYER = "Player";

      private Fighter fighter;
      private Health health;
      private Mover mover;
      private GameObject player;

      private Vector3 guardPosition; 
      private float timeSinceLastSawPlayer = Mathf.Infinity;
      private int currentWaypointIndex = 0;

      private void Awake()
      {
         fighter = GetComponent<Fighter>();
         health = GetComponent<Health>();
         mover = GetComponent<Mover>();
         player = GameObject.FindWithTag(PLAYER);

         guardPosition = transform.position;
      }

      private void Update()
      {
         if(health.GetIsDead()) return;
         
         if(InAttackRangeOfPlayer() && fighter.CanAttack(player))
         {
            timeSinceLastSawPlayer = 0f;
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

      }

      private void PatrolBehaviour()
      {
         Vector3 nextPosition = guardPosition;

         if(patrolPath != null)
         {
            if(AtWaypoint())
            {
               CycleWaypoint();
            }
            nextPosition = GetCurrentWaypoint();
         }

         mover.StartMoveAction(nextPosition);
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
         timeSinceLastSawPlayer += Time.deltaTime;
         GetComponent<ActionScheduler>().CancelCurrentAction();
      }

      private void AttackBehaviour()
      {
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
