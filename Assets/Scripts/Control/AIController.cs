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
      
      private const string PLAYER = "Player";

      private Fighter fighter;
      private Health health;
      private Mover mover;
      private GameObject player;

      private Vector3 guardPosition;

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
            fighter.Attack(player);
         }
         else
         {
            if(guardPosition != transform.position)
            {  
               mover.StartMoveAction(guardPosition);    
            }
         }

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
