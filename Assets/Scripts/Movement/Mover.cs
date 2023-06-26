using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using RPG.Core;

namespace RPG.Movement
{
   public class Mover : MonoBehaviour, IAction
   {
      private NavMeshAgent navMeshAgent;

      private void Awake()
      {
         navMeshAgent = GetComponent<NavMeshAgent>();
      }

      private void Start()
      {
         GetComponent<Health>().OnZeroHealth += Health_OnZeroHealth;
      }

      private void Health_OnZeroHealth(object sender, EventArgs e)
      {
         navMeshAgent.enabled = false;
      }

      public void StartMoving(Vector3 destination)
      {
         MoveTo(destination);
         GetComponent<ActionScheduler>().StartAction(this);
      }

      public void MoveTo(Vector3 destination)
      {
         navMeshAgent.destination = destination;
         navMeshAgent.isStopped = false;
      }

      public void Cancel()
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
