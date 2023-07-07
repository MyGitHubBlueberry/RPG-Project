using RPG.Tags;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
   public class HealthDisplay : MonoBehaviour
   {
      private const string HEALTH_LABEL = "Health: ";

      private Health health;
      private TextMeshProUGUI healthText;

      private void Awake()
      {
         health = GameObject.FindWithTag(Tag.Player.ToString()).GetComponent<Health>();
         healthText = GetComponent<TextMeshProUGUI>();
      }

      private void Update()
      {
         healthText.text = string.Format(HEALTH_LABEL + $"{health.GetPercentage():0}%");
      }
   }
}
