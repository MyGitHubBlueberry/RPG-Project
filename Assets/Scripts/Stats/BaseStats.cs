using UnityEngine;

namespace RPG.Stats
{
   public class BaseStats : MonoBehaviour
   {
      [Range(1,99)]
      [SerializeField] private int level = 1;
      [SerializeField] private CharacterClass characterClass;
      [SerializeField] private Progression progression;

      public float GetHealth()
      {
         return progression.GetHealth(characterClass, level);
      }

      public float GetExperienceReward()
      {
         return 10;
      }
   }
}
