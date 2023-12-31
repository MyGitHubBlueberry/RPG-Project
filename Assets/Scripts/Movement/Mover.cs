using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using Newtonsoft.Json.Linq;
using RPG.Attributes;
using RPG.Animation;

namespace RPG.Movement
{
   public class Mover : MonoBehaviour, IAction, ISaveable
   {
      [SerializeField] private float maxSpeed = 6f;
      [SerializeField] private float maxNavMeshPathLength = 40f;

      private Animation.RuntimeUpdateAnimationHandler updateAnimationHandler;
      private NavMeshAgent navMeshAgent;
      private Health health;
      

      private void Awake()
      {
         navMeshAgent = GetComponent<NavMeshAgent>();
         health = GetComponent<Health>();
         updateAnimationHandler = GetComponent<Animation.RuntimeUpdateAnimationHandler>();
      }

      private void OnEnable()
      {
         updateAnimationHandler.OnSetFloatParametersRequiered += SetFlaotParameters;
         GetComponent<Health>().OnZeroHealth += DisableNavMesh;
      }

      private void Update()
      {
         if(health.IsDead()) return;
      }

      private void OnDisable()
      {
         updateAnimationHandler.OnSetFloatParametersRequiered -= SetFlaotParameters;
         GetComponent<Health>().OnZeroHealth -= DisableNavMesh;
      }

      private void SetFlaotParameters(object sender, RuntimeUpdateAnimationHandler.OnSetFloatParametersRequieredEventArgs e)
      {
         e.SetFloatParameters.Invoke(Animation.AnimatorParameters.Value.forwardSpeed, health.IsAlive, GetMovementSpeed);
         
         updateAnimationHandler.OnSetFloatParametersRequiered -= SetFlaotParameters;
      }


      private float GetPathLength(NavMeshPath path)
      {
         float pathLength = 0;

         for (int index = 0, nextIndex = 1; nextIndex < path.corners.Length; index++, nextIndex++)
         {
            pathLength += Vector3.Distance(path.corners[index], path.corners[nextIndex]);
         }
         return pathLength;
      }
      
      private float GetMovementSpeed()
      {
         Vector3 velocity = navMeshAgent.velocity;
         Vector3 localVelocity = transform.InverseTransformDirection(velocity);
         float speed = localVelocity.z;

         return speed;
      }

      private void DisableNavMesh()
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

      public bool CanMoveTo(Vector3 destination)
      {
         NavMeshPath path = new NavMeshPath();
         bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
         if(!hasPath)return false;
         if(path.status != NavMeshPathStatus.PathComplete) return false;
         if(GetPathLength(path) > maxNavMeshPathLength) return false;

         return true;
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
