using System;
using UnityEngine;

namespace RPG.Animation
{
   public class RuntimeUpdateAnimationHandler : MonoBehaviour
   {
      public event Action<Action<Action<float, AnimatorParameters.Value>, Action<int, AnimatorParameters.Value>>> OnExecuteMethodsRequiered;
      public event Action<Action<AnimatorParameters.Value, Func<bool>, Func<float>>> OnSetFloatParametersRequiered;
      public event Action<Action<AnimatorParameters.Value, Func<bool>, Func<int>>> OnSetIntParametersRequiered;
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
         OnExecuteMethodsRequiered?.Invoke(SetExecuteMethods);
         OnSetFloatParametersRequiered?.Invoke(SetFlaotParameters);
         OnSetIntParametersRequiered?.Invoke(SetIntParameters);
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
      public void SetFlaotParameters(AnimatorParameters.Value valueParameter, Func<bool> condition, Func<float> value)
      {
         if(isParametersSet) return;

         SetDefaultParameters(valueParameter, condition);
         floatValue = value;
         isFloatParameter = true;
      } 

      public void SetIntParameters(AnimatorParameters.Value valueParameter, Func<bool> condition, Func<int> value)
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
