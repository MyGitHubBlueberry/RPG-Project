using System;
using UnityEngine;

namespace RPG.Animation
{
   public class AnimatorController : MonoBehaviour
   {
      private IOverrideRuntimeAnimatorControllerEvent[] runtimeAnimatorOverrides;
      private RuntimeUpdateAnimationHandler runtimeUpdateAnimationHandler;
      private IAnimationTriggerEvent[] animationTriggerEvents;
      private Animator animator;


      private void Awake()
      {
         runtimeUpdateAnimationHandler= GetComponent<RuntimeUpdateAnimationHandler>();
         animator = GetComponent<Animator>();

         runtimeAnimatorOverrides = GetComponents<IOverrideRuntimeAnimatorControllerEvent>();
         animationTriggerEvents = GetComponents<IAnimationTriggerEvent>();
      }

      private void OnEnable()
      {
         runtimeUpdateAnimationHandler.OnExecuteMethodsRequiered += SetExecuteMethods;


         foreach(IAnimationTriggerEvent animationEvent in animationTriggerEvents)
         {
            animationEvent.OnResetSetAnimationTriggerRequest += ResetSetTrigger;
         }

         foreach(IOverrideRuntimeAnimatorControllerEvent runtimeOverride in runtimeAnimatorOverrides)
         {
            runtimeOverride.OnOverrideRuntimeAnimatorControllerRequest += OverrideRuntimeAnimatorController;
         }
      }

      private void SetExecuteMethods(Action<Action<float, AnimatorParameters.Value>, Action<int, AnimatorParameters.Value>> action)
      {
         action.Invoke(SetFloat,SetInt);
         
         runtimeUpdateAnimationHandler.OnExecuteMethodsRequiered -= SetExecuteMethods;
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
         animator.ResetTrigger(e.resetTrigger.ToString());
         animator.SetTrigger(e.setTrigger.ToString());
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

      private void SetFloat(float value, AnimatorParameters.Value valueParameter)
      {
         animator.SetFloat(valueParameter.ToString(), value);
      }
      private void SetInt(int value, AnimatorParameters.Value valueParameter)
      {
         animator.SetInteger(valueParameter.ToString(), value);
      }
   }
}