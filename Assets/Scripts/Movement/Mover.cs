using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using RPG.Combat;
using RPG.Core;

namespace RPG.Movement
{
   public class Mover : MonoBehaviour
   {
      private NavMeshAgent navMeshAgent;

      private void Start()
      {
         navMeshAgent = GetComponent<NavMeshAgent>();
      }

      public void StartMoving(Vector3 destination)
      {
         MoveTo(destination);
         GetComponent<ActionScheduler>().StartAction(this);
         GetComponent<Fighter>().Cancel();
      }

      public void MoveTo(Vector3 destination)
      {
         navMeshAgent.destination = destination;
         navMeshAgent.isStopped = false;
      }

      public void Stop()
      {
         navMeshAgent.isStopped = true;
      }

      public float GetMovementSpeed()
      {
         Vector3 velocity = navMeshAgent.velocity;
         Vector3 localVelocity = transform.InverseTransformDirection(velocity);
         float speed = localVelocity.z;

         return speed;
      }
   }
}
