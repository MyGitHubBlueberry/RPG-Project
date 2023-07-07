using RPG.Attributes;
using RPG.Tags;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
   public class EnemyHealthDisplay : MonoBehaviour
   {
      private const string ENEMY_LABEL = "Enemy: ";

      private Fighter fighter;
      private Health health;
      private TextMeshProUGUI healthText;

      private void Awake()
      {
         fighter = GameObject.FindGameObjectWithTag(Tag.Player.ToString()).GetComponent<Fighter>();
         healthText = GetComponent<TextMeshProUGUI>();
      }

      private void Update()
      {
         health = fighter.GetTarget();

         healthText.text = string.Format(ENEMY_LABEL + GetDisplayValue());
      }

      private string GetDisplayValue()
      {
         return ((health != null) ? $"{health.GetPercentage():0}%" : "N/A");
      }
   }
}