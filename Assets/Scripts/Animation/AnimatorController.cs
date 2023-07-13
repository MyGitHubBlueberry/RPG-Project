using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;

namespace RPG.Animation
{
   public class AnimatorController : MonoBehaviour
   {
      private IAnimationTriggerEvent[] animationTriggerEvents;
      private IOverrideRuntimeAnimatorControllerEvent[] runtimeAnimatorOverrides;
      private Animator animator;
      private Mover mover;
      private Health health;
      private const string FORWARD_SPEED = "forwardSpeed";


      private void Awake()
      {
         animator = GetComponent<Animator>();
         mover = GetComponent<Mover>();
         health = GetComponent<Health>();

         animationTriggerEvents = GetComponents<IAnimationTriggerEvent>();
         runtimeAnimatorOverrides = GetComponents<IOverrideRuntimeAnimatorControllerEvent>();
      }

      private void OnEnable()
      {
         foreach(IAnimationTriggerEvent animationEvent in animationTriggerEvents)
         {
            animationEvent.OnResetSetAnimationTriggerRequest += ResetSetTrigger;
         }

         foreach(IOverrideRuntimeAnimatorControllerEvent runtimeOverride in runtimeAnimatorOverrides)
         {
            runtimeOverride.OnOverrideRuntimeAnimatorControllerRequest += OverrideRuntimeAnimatorController;
         }
      }

      private void Update()
      {
         if(health.GetIsDead()) return;

         UpdateMovement(mover.GetMovementSpeed());      
      }

      private void OnDisable()
      {
         foreach(IAnimationTriggerEvent animationEvent in animationTriggerEvents)
         {
            animationEvent.OnResetSetAnimationTriggerRequest -= ResetSetTrigger;
         }

         foreach(IOverrideRuntimeAnimatorControllerEvent runtimeOverride in runtimeAnimatorOverrides)
         {
            runtimeOverride.OnOverrideRuntimeAnimatorControllerRequest -= OverrideRuntimeAnimatorController;
         }
      }

      private void ResetSetTrigger(object sender, IAnimationTriggerEvent.OnResetSetAnimationTriggerRequestEventArgs e)
      {
         animator.ResetTrigger(e.resetTriggerCondition.ToString());
         animator.SetTrigger(e.setTriggerCondition.ToString());
      }

      private void OverrideRuntimeAnimatorController(AnimatorOverrideController animatorOverride)
      {
         if (animatorOverride != null)
         {
            animator.runtimeAnimatorController = animatorOverride;
         }
         else if (IsAnimatorOverridden(out AnimatorOverrideController previousAnimatorOverride))
         {
            animator.runtimeAnimatorController = previousAnimatorOverride.runtimeAnimatorController;
         }
      }
      
      private bool IsAnimatorOverridden(out AnimatorOverrideController animatorOverride)
      {
         animatorOverride = animator.runtimeAnimatorController as AnimatorOverrideController;
         return animatorOverride != null;
      }

      private void UpdateMovement(float movementSpeed)
      {
         animator.SetFloat(FORWARD_SPEED, movementSpeed);
      }
   }
}