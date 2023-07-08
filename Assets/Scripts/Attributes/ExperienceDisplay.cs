using RPG.Tags;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
   public class ExperienceDisplay : MonoBehaviour
   {
      private const string EPERIENCE_LABEL = "XP: ";

      private Experience experience;
      private TextMeshProUGUI experienceText;

      private void Awake()
      {
         experience = GameObject.FindWithTag(Tag.Player.ToString()).GetComponent<Experience>();
         experienceText = GetComponent<TextMeshProUGUI>();
      }

      private void Update()
      {
         experienceText.text = string.Format(EPERIENCE_LABEL + $"{experience.GetExperience():0}");
      }
   }
}
