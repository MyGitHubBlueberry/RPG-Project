using System;
using UnityEngine;

namespace RPG.Animation
{
   public class RuntimeUpdateAnimationHandler : MonoBehaviour
   {
      public event EventHandler<OnExecuteMethodsRequieredEventArgs> OnExecuteMethodsRequiered;
      public class OnExecuteMethodsRequieredEventArgs : EventArgs
      {
         public Action<Action<float, AnimatorParameters.Value>, Action<int, AnimatorParameters.Value>> SetExecuteMethods;
      }
      
      public event EventHandler<OnSetFloatParametersRequieredEventArgs> OnSetFloatParametersRequiered;
      public class OnSetFloatParametersRequieredEventArgs : EventArgs
      {
         public Action<AnimatorParameters.Value, Func<bool>, Func<float>> SetFloatParameters;
      }

      public event EventHandler<OnSetIntParametersRequieredEventArgs> OnSetIntParametersRequiered;
      public class OnSetIntParametersRequieredEventArgs : EventArgs
      {
         public Action<AnimatorParameters.Value, Func<bool>, Func<int>> SetIntParameters;
      }

      private Action<float, AnimatorParameters.Value> floatExecutor;
      private Action<int, AnimatorParameters.Value> intExecutor;
      private AnimatorParameters.Value valueParameter;
      private Func<float> floatValue;
      private Func<bool> condition;
      private Func<int> intValue;
      private bool isFloatParameter;
      private bool isParametersSet;

      private void Start()
      {
         OnExecuteMethodsRequiered?.Invoke(this, new OnExecuteMethodsRequieredEventArgs 
         {
            SetExecuteMethods = SetExecuteMethods
         });
         OnSetFloatParametersRequiered?.Invoke(this, new OnSetFloatParametersRequieredEventArgs
         {
            SetFloatParameters = SetFloatParameters,
         });
         OnSetIntParametersRequiered?.Invoke(this, new OnSetIntParametersRequieredEventArgs
         {
            SetIntParameters = SetIntParameters,
         });
      }

      private void Update()
      {
         if(!isParametersSet) return;
         if(floatExecutor == null || intExecutor == null) return;
         if(!condition.Invoke()) return;

         if(isFloatParameter)
         {
            float value = floatValue.Invoke();
            floatExecutor.Invoke(value, valueParameter);
         }
         else
         {
            int value = intValue.Invoke();
            intExecutor.Invoke(value, valueParameter);
         }
      }
      private void SetDefaultParameters(AnimatorParameters.Value valueParameter, Func<bool> condition)
      {
         this.valueParameter = valueParameter;
         this.condition = condition;
         isParametersSet = true;
      } 

      private void SetFloatParameters(AnimatorParameters.Value valueParameter, Func<bool> condition, Func<float> value)
      {
         if(isParametersSet) return;

         SetDefaultParameters(valueParameter, condition);
         floatValue = value;
         isFloatParameter = true;
      } 

      private void SetIntParameters(AnimatorParameters.Value valueParameter, Func<bool> condition, Func<int> value)
      {
         if(isParametersSet) return;

         SetDefaultParameters(valueParameter, condition);
         intValue = value;
         isFloatParameter = false;
      }

      private void SetExecuteMethods(Action<float, AnimatorParameters.Value> floatExecutor, Action<int, AnimatorParameters.Value> intExecutor)
      {
         this.intExecutor = intExecutor;
         this.floatExecutor = floatExecutor;
      }
   }
}
