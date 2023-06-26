using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.Control
{
   public class AIController : MonoBehaviour
   {
      [SerializeField] private float chaseDistance = 5f;
      
      private const string PLAYER = "Player";

      private Fighter fighter;
      private GameObject player;


      private void Awake()
      {
         fighter = GetComponent<Fighter>();
         player = GameObject.FindWithTag(PLAYER);

      }
      private void Update()
      {
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

   }
}
