using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
   public class Mover : MonoBehaviour, IAction
   {
      [SerializeField] private float maxSpeed = 6f;

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

      public void StartMoveAction(Vector3 destination, float speedCoefficient)
      {
         MoveTo(destination, speedCoefficient);
         GetComponent<ActionScheduler>().StartAction(this);
      }

      public void MoveTo(Vector3 destination, float speedCoefficient)
      {
         navMeshAgent.destination = destination;
         navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedCoefficient);
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
