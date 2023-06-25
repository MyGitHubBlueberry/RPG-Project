using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
   public class AIController : MonoBehaviour
   {
      [SerializeField] private float chaseDistance = 5f;
      
      private const string PLAYER = "Player";

      private void Update()
      {
         if(DistanceToPlayer() < chaseDistance)
         {
            print("Chase " + gameObject.name);
         }

      }

      private float DistanceToPlayer()
      {
         GameObject player = GameObject.FindWithTag(PLAYER);
         return Vector3.Distance(transform.position, player.transform.position);
      }

   }
}
