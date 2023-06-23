using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace RPG.Movement
{
   public class Mover : MonoBehaviour
   {
      public void MoveTo(Vector3 destination)
      {
         GetComponent<NavMeshAgent>().destination = destination;
      }

      public float GetMovementSpeed()
      {
         Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
         Vector3 localVelocity = transform.InverseTransformDirection(velocity);
         float speed = localVelocity.z;

         return speed;
      }
   }
}
