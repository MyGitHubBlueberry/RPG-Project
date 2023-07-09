using System;
using RPG.Tags;
using UnityEngine;

namespace RPG.Stats
{
   public class BaseStats : MonoBehaviour
   {
      //public event Action OnLevelUp;


      [Range(1,99)]
      [SerializeField] private int startingLevel = 1;
      [SerializeField] private CharacterClass characterClass;
      [SerializeField] private Progression progression;
      [SerializeField] private AnimationCurve levelCurve = new AnimationCurve();

      public float GetStat(Stat stat)
      {
         return progression.GetStat(stat, characterClass, GetLevel());
      }

      public int GetLevel()
      {
         if(!TryGetComponent<Experience>(out Experience experience)) return startingLevel;

         float currentXP = experience.GetExperience();
         int penultimateLevel = progression.GetPenultimateLevel(Stat.ExperienceToLevelUp, characterClass);
         for (int level = 1; level <= penultimateLevel; level++)
         {
            float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
            if(XPToLevelUp > currentXP)
            {
               return level;
            }
         }

         return penultimateLevel + 1;
      }
   }
}
