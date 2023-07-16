using UnityEngine;
using TMPro;
using RPG.Animation;
using System;

namespace RPG.UI
{
   public class DamageText : MonoBehaviour, IAnimationTriggerEvent
   {
      public event EventHandler<IAnimationTriggerEvent.OnResetSetAnimationTriggerRequestEventArgs> OnResetSetAnimationTriggerRequest;

      [SerializeField] private TextMeshProUGUI damageText;

      public void InvokeTextAnimation()
      {
         //damageText.text = $"{damage.ToString():0.0}";
         OnResetSetAnimationTriggerRequest?.Invoke(this, new IAnimationTriggerEvent.OnResetSetAnimationTriggerRequestEventArgs
         {
            setTrigger = AnimatorParameters.Trigger.damageTextPopup,
            resetTrigger = AnimatorParameters.Trigger.damageTextPopup,
         });
      }

      public void DestroyText()
      {
         Destroy(gameObject);
      }
   }
}
