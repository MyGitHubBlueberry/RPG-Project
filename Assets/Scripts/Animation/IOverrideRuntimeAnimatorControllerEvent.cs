using System;
using UnityEngine;

namespace RPG.Animation
{
   public interface IOverrideRuntimeAnimatorControllerEvent
   {
      public event Action<AnimatorOverrideController> OnOverrideRuntimeAnimatorControllerRequest;
   }
}