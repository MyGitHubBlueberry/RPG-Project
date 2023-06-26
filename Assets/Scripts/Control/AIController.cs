using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
   public class AIController : MonoBehaviour
   {
      [SerializeField] private float chaseDistance = 5f;
      
      private const string PLAYER = "Player";

      private Fighter fighter;
      private Health health;
      private GameObject player;


      private void Awake()
      {
         fighter = GetComponent<Fighter>();
         health = GetComponent<Health>();
         player = GameObject.FindWithTag(PLAYER);
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
            fighter.Cancel();
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
