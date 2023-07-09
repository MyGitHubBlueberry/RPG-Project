using RPG.Tags;
using TMPro;
using UnityEngine;
using System.Text;

namespace RPG.Stats
{
   public class ExperienceDisplay : MonoBehaviour
   {
       private StringBuilder stringBuilder = new StringBuilder("XP: ", 10);
      private Experience experience;
      private TextMeshProUGUI experienceText;
      private int startBuilderLength;

      private void Awake()
      {
         experience = GameObject.FindWithTag(Tag.Player.ToString()).GetComponent<Experience>();
         experienceText = GetComponent<TextMeshProUGUI>();
         startBuilderLength = stringBuilder.Length;
      }

      private void Start()
      {
         experience.OnExperienceChanged += Experience_OnExperienceChanged;
      }

      private void Experience_OnExperienceChanged()
      {
         UpdateDisplay();
      }

      private void UpdateDisplay()
      {
         string xpAmount = $"{experience.GetExperience():0}";
         stringBuilder.Append(xpAmount);
         experienceText.text = stringBuilder.ToString();
         stringBuilder.Remove(startBuilderLength, xpAmount.Length);
      }
   }
}