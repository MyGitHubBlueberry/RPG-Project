using UnityEngine;

namespace RPG.Animation
{
   public class AnimatorController : MonoBehaviour
   {
      private IOverrideRuntimeAnimatorControllerEvent[] runtimeAnimatorOverrides = null;
      private RuntimeUpdateAnimationHandler runtimeUpdateAnimationHandler = null;
      private IAnimationTriggerEvent[] animationTriggerEvents = null;
      private Animator animator;


      private void Awake()
      {
         runtimeUpdateAnimationHandler = GetComponent<RuntimeUpdateAnimationHandler>();
         animator = GetComponent<Animator>();

         runtimeAnimatorOverrides = GetComponents<IOverrideRuntimeAnimatorControllerEvent>();
         animationTriggerEvents = GetComponents<IAnimationTriggerEvent>();
      }

      private void OnEnable()
      {
         if(runtimeUpdateAnimationHandler is not null)
         {
            runtimeUpdateAnimationHandler.OnExecuteMethodsRequiered += SetExecuteMethods;
         }
         
         if(animationTriggerEvents is not null)
         {
            foreach(IAnimationTriggerEvent animationEvent in animationTriggerEvents)
            {
               animationEvent.OnResetSetAnimationTriggerRequest += ResetSetTrigger;
            }
         }

         if(runtimeAnimatorOverrides is not null)
         {
            foreach(IOverrideRuntimeAnimatorControllerEvent runtimeOverride in runtimeAnimatorOverrides)
            {
               runtimeOverride.OnOverrideRuntimeAnimatorControllerRequest += OverrideRuntimeAnimatorController;
            }
         }
      }

      private void OnDisable()
      {
         if(runtimeUpdateAnimationHandler is not null)
         {
            runtimeUpdateAnimationHandler.OnExecuteMethodsRequiered -= SetExecuteMethods;
         }

         if(animationTriggerEvents is not null)
         {
            foreach(IAnimationTriggerEvent animationEvent in animationTriggerEvents)
            {
               animationEvent.OnResetSetAnimationTriggerRequest -= ResetSetTrigger;
            }
         }

         if(runtimeAnimatorOverrides is not null)
         {
            foreach(IOverrideRuntimeAnimatorControllerEvent runtimeOverride in runtimeAnimatorOverrides)
            {
               runtimeOverride.OnOverrideRuntimeAnimatorControllerRequest -= OverrideRuntimeAnimatorController;
            }
         }
      }
      private void SetExecuteMethods(object sender, RuntimeUpdateAnimationHandler.OnExecuteMethodsRequieredEventArgs e)
      {
         e.SetExecuteMethods(SetFloat, SetInt);

         runtimeUpdateAnimationHandler.OnExecuteMethodsRequiered -= SetExecuteMethods;
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

      private void SetFloat(float value, AnimatorParameters.Value valueParameter)
      {
         animator.SetFloat(valueParameter.ToString(), value);
      }
      private void SetInt(int value, AnimatorParameters.Value valueParameter)
      {
         animator.SetInteger(valueParameter.ToString(), value);
      }
      
      private bool IsAnimatorOverridden(out AnimatorOverrideController animatorOverride)
      {
         animatorOverride = animator.runtimeAnimatorController as AnimatorOverrideController;
         return animatorOverride != null;
      }
   }
}