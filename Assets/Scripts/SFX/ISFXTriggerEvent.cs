using System;

namespace RPG.SFX
{
   interface ISFXEvent
   {
      public event Action<SFXParameter> OnSFXTriggerRequest;
   }

}