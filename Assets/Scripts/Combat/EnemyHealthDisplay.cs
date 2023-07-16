using System.Text;
using RPG.Attributes;
using RPG.Tags;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
   public class EnemyHealthDisplay : MonoBehaviour
   {
      private StringBuilder stringBuilder = new StringBuilder("Enemy: ", 11);
      private int startBuilderLength;
      private Fighter fighter;
      private Health targetHealth;
      private TextMeshProUGUI healthText;

      private void Awake()
      {
         fighter = GameObject.FindGameObjectWithTag(Tag.Player.ToString()).GetComponent<Fighter>();
         healthText = GetComponent<TextMeshProUGUI>();
         startBuilderLength = stringBuilder.Length;
      }

      private void OnEnable()
      {
         fighter.OnTargetSet += UpdateDisplayAndSubscribe;
         fighter.OnAttackCanceled += UnsubscribeAndUpdateDisplay;         
      }

      private void OnDisable()
      {
         fighter.OnTargetSet -= UpdateDisplayAndSubscribe;
         fighter.OnAttackCanceled -= UnsubscribeAndUpdateDisplay;
      }

      private void UnsubscribeAndUpdateDisplay()
      {
         targetHealth.OnHealthRegenerated -= UpdateDisplay;
         targetHealth.OnTakeDamage -= _ => UpdateDisplay();
         UpdateDisplay();
      }

      private void UpdateDisplayAndSubscribe()
      {
         UpdateDisplay();
         targetHealth.OnHealthRegenerated += UpdateDisplay;
         targetHealth.OnTakeDamage += _ => UpdateDisplay();;
      }

      private void UpdateDisplay()
      {
         this.targetHealth = fighter.GetTarget();

         string target = GetDisplayValue();
         stringBuilder.Append(target);
         healthText.text = stringBuilder.ToString();
         stringBuilder.Remove(startBuilderLength, target.Length);
      }

      private string GetDisplayValue()
      {
         return ((targetHealth != null) ? $"{targetHealth.GetPercentage():0}%" : "N/A");
      }
   }
}