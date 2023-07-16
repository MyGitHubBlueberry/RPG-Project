using UnityEngine;
using TMPro;
using System;

namespace RPG.UI
{
   public class DamageText : MonoBehaviour
   {
      [SerializeField] private TextMeshProUGUI damageText;

      public void SetValue(float amount)
      {
         damageText.text = String.Format("{0:0.0}", amount);
      }

      public void DestroyText()
      {
         Destroy(gameObject);
      }
   }
}
