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

      private void Start()
      {
         fighter.OnTargetSet += () =>
         {
            UpdateDisplay();
            targetHealth.OnHealthChanged += UpdateDisplay;
         };

         fighter.OnAttackCanceled +=  () =>
         {
            targetHealth.OnHealthChanged -= UpdateDisplay;
            UpdateDisplay();
         };

         UpdateDisplay();
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