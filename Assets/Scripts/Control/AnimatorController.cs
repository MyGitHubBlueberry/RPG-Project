using UnityEngine;
using UnityEngine.AI;
using RPG.Movement;

namespace RPG.Control
{
   public class AnimatorController : MonoBehaviour
   {
      private const string FORWARD_SPEED = "forwardSpeed";


      private void Update()
      {
         UpdateAnimator(GetComponent<Mover>().GetMovementSpeed());      
      }

      private void UpdateAnimator(float movementSpeed)
      {
         GetComponent<Animator>().SetFloat(FORWARD_SPEED, movementSpeed);
      }
   }
}