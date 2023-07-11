using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using Newtonsoft.Json.Linq;
using RPG.Attributes;

namespace RPG.Movement
{
   public class Mover : MonoBehaviour, IAction, ISaveable
   {
      [SerializeField] private float maxSpeed = 6f;

      private NavMeshAgent navMeshAgent;
      private Health health;

      private void Awake()
      {
         navMeshAgent = GetComponent<NavMeshAgent>();
         health = GetComponent<Health>();
      }

      private void OnEnable()
      {
         GetComponent<Health>().OnZeroHealth += Health_OnZeroHealth;
      }

      private void OnDisable()
      {
         GetComponent<Health>().OnZeroHealth -= Health_OnZeroHealth;
      }

      private void Health_OnZeroHealth()
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

      public JToken CaptureAsJToken()
      {
         return transform.position.ToToken();
      }

      public void RestoreFromJToken(JToken state)
      {
         navMeshAgent.enabled = false;
         transform.position = state.ToVector3();
         navMeshAgent.enabled = true;
         GetComponent<ActionScheduler>().CancelCurrentAction();
      }

   }
}
