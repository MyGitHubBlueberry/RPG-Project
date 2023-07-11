
using System;
using System.Text;
using RPG.Tags;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
   public class HealthDisplay : MonoBehaviour
   {
      private StringBuilder stringBuilder = new StringBuilder("Health: ", 13);
      private Health health;
      private TextMeshProUGUI healthText;
      private int startBuilderLength;

      private void Awake()
      {
         health = GameObject.FindWithTag(Tag.Player.ToString()).GetComponent<Health>();
         healthText = GetComponent<TextMeshProUGUI>();
         startBuilderLength = stringBuilder.Length;
      }

      private void OnEnable()
      {
         health.OnHealthChanged += UpdateDispaly;
         health.OnZeroHealth += UpdateDispaly;
      }

      private void Start() => UpdateDispaly();

      private void OnDisable()
      {
         health.OnHealthChanged -= UpdateDispaly;
         health.OnZeroHealth -= UpdateDispaly;
      }

      private void UpdateDispaly()
      {
         string healthPersent = $"{health.GetHealth():0}/{health.GetMaxHealth():0}";
         stringBuilder.Append(healthPersent);
         healthText.text = stringBuilder.ToString();
         stringBuilder.Remove(startBuilderLength, healthPersent.Length);
      }
   }
}
