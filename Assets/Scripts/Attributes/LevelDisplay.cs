using System;
using System.Text;
using RPG.Stats;
using RPG.Tags;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
   public class LevelDisplay : MonoBehaviour
   {
      private StringBuilder stringBuilder = new StringBuilder("Level: ", 10);
      private BaseStats baseStats;
      private TextMeshProUGUI levelText;
      private int startBuilderLength;
   
      private void Awake()
      {
         baseStats = GameObject.FindWithTag(Tag.Player.ToString()).GetComponent<BaseStats>();
         levelText = GetComponent<TextMeshProUGUI>();
         startBuilderLength = stringBuilder.Length;
      }
      
      private void OnEnable()
      {
         baseStats.OnLevelUp += UpdateDisplay;
         baseStats.OnRestoreLevel += UpdateDisplay;
      }

      private void Start() => UpdateDisplay();

      private void OnDisable()
      {
         baseStats.OnLevelUp -= UpdateDisplay;
         baseStats.OnRestoreLevel -= UpdateDisplay;
      }

      private void UpdateDisplay()
      {
         string level = $"{baseStats.GetLevel():0}";
         stringBuilder.Append(level);
         levelText.text = stringBuilder.ToString();
         stringBuilder.Remove(startBuilderLength, level.Length);
      }
   }
}