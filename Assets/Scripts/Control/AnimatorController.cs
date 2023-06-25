using UnityEngine;
using UnityEngine.AI;
using RPG.Movement;
using RPG.Combat;
using System;

namespace RPG.Control
{
   public class AnimatorController : MonoBehaviour
   {
      private Animator animator;
      private Mover mover;
      private Fighter fighter;
      private const string FORWARD_SPEED = "forwardSpeed";
      private const string ATTACK = "attack";


      private void Awake()
      {
         animator = GetComponent<Animator>();
         mover = GetComponent<Mover>();
         fighter = GetComponent<Fighter>();
      }

      private void Start()
      {
         fighter.OnAttack += Fighter_OnAttack;
      }

      private void Fighter_OnAttack(object sender, EventArgs e)
      {
         animator.SetTrigger(ATTACK);
      }

      private void Update()
      {
         UpdateMovement(mover.GetMovementSpeed());      
      }

      private void UpdateMovement(float movementSpeed)
      {
         animator.SetFloat(FORWARD_SPEED, movementSpeed);
      }
   }
}