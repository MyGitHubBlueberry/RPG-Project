
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
         health.OnHealthRegenerated += UpdateDisplay;
         health.OnTakeDamage += _ => UpdateDisplay();
         health.OnZeroHealth += UpdateDisplay;
      }

      private void Start() => UpdateDisplay();

      private void OnDisable()
      {
         health.OnHealthRegenerated -= UpdateDisplay;
         health.OnTakeDamage -= _ => UpdateDisplay();
         health.OnZeroHealth -= UpdateDisplay;
      }

      private void UpdateDisplay()
      {
         string healthPersent = $"{health.GetHealth():0}/{health.GetMaxHealth():0}";
         stringBuilder.Append(healthPersent);
         healthText.text = stringBuilder.ToString();
         stringBuilder.Remove(startBuilderLength, healthPersent.Length);
      }
   }
}
