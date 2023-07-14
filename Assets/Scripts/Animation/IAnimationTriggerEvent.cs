using System;

namespace RPG.Animation
{
   public interface IAnimationTriggerEvent
   {
      /// <summary>
      /// Event resets one trigger and sets anoter.
      /// </summary>
      public event EventHandler<OnResetSetAnimationTriggerRequestEventArgs> OnResetSetAnimationTriggerRequest;
      public class OnResetSetAnimationTriggerRequestEventArgs : EventArgs
      {
         public AnimatorParameters.Trigger resetTrigger;
         public AnimatorParameters.Trigger setTrigger;
      }
   }
}