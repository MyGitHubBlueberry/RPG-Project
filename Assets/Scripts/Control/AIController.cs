using System;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Tags;
using UnityEngine;
using RPG.Attributes;
using GameDevTV.Utils;

namespace RPG.Control
{
   public class AIController : MonoBehaviour
   {
      [SerializeField] private float chaseDistance = 5f;
      [SerializeField] private float shoutDistance = 5f;
      [SerializeField] private float suspicionTime = 3f;
      [SerializeField] private float aggroCooldownTime = 5f;
      [SerializeField] private PatrolPath patrolPath;
      [SerializeField] private float waypointTolerance = 1f;
      [SerializeField] private float waypointDwellTime = 3f;
      [Range(0,1)]
      [SerializeField] private float patrolSpeedCoefficient = 0.2f;

      
      private Fighter fighter;
      private Health health;
      private Mover mover;
      private GameObject player;

      private LazyValue<Vector3> guardPosition; 
      private float timeSinceLastSawPlayer = Mathf.Infinity;
      private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
      private float timeSinceAggrevated = Mathf.Infinity;
      private int currentWaypointIndex = 0;

      private void Awake()
      {
         fighter = GetComponent<Fighter>();
         health = GetComponent<Health>();
         mover = GetComponent<Mover>();
         player = GameObject.FindWithTag(Tag.Player.ToString());

         guardPosition = new LazyValue<Vector3>(GetGuardPosition);
      }

      private void Start()
      {
         guardPosition.ForceInit();
      }

      private void OnEnable()
      {
         health.OnTakeDamage += _ => Aggrevate();
      }

      private void Update()
      {
         if (health.IsDead()) return;

         if (IsAggrevated() && fighter.CanAttack(player))
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

      private void OnDisable()
      {
         health.OnTakeDamage -= _ => Aggrevate();
      }

      private void Aggrevate()
      {
         timeSinceAggrevated = 0f;
      }

      private Vector3 GetGuardPosition()
      {
         return transform.position;
      }

      private void UpdateTimers()
      {
         timeSinceLastSawPlayer += Time.deltaTime;
         timeSinceArrivedAtWaypoint += Time.deltaTime;
         timeSinceAggrevated += Time.deltaTime;
      }

      private void PatrolBehaviour()
      {
         Vector3 nextPosition = guardPosition.value;

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

         AggrevateNearbyEnemies();
      }

      private void AggrevateNearbyEnemies()
      {
         RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0f);
         foreach(RaycastHit hit in hits)
         {
            if(!hit.transform.TryGetComponent<AIController>(out AIController controller)) continue;

            controller.Aggrevate();
         }
      }

      private bool IsAggrevated()
      {
         float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
         return distanceToPlayer < chaseDistance || timeSinceAggrevated < aggroCooldownTime;
      }

      //*Called by Unity
      private void OnDrawGizmosSelected()
      {
         Gizmos.color = Color.blue;
         Gizmos.DrawWireSphere(transform.position, chaseDistance);
      }
   }
}
