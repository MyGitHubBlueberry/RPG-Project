using System;
using UnityEngine;

namespace RPG.Stats
{
   public class BaseStats : MonoBehaviour
   {
      [Range(1,99)]
      [SerializeField] private int startingLevel = 1;
      [SerializeField] private CharacterClass characterClass;
      [SerializeField] private Progression progression;
      [SerializeField] private GameObject levelUpParticleEffect;
      
      public event Action OnLevelUp;

      private Experience experience;
      private int currentLevel = 0;



      private void Awake()
      {
         experience = GetComponent<Experience>();
      }

      private void Start()
      {
         currentLevel = CalculateLevel();
         if(experience != null)
         {
            experience.OnExperienceGained += UpdateLevel;
         }
      }

      private int CalculateLevel()
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

      private void UpdateLevel()
      {
         int newLevel = CalculateLevel();
         if(newLevel > currentLevel)
         {
            currentLevel = newLevel;
            InstantiateLevelUpEffect();
            OnLevelUp?.Invoke();
         }
      }

      private void InstantiateLevelUpEffect()
      {
         Instantiate(levelUpParticleEffect, transform);
      }
      private float GetAdditionalModifier(Stat stat)
      {
         float total = 0;
         foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
         {
            foreach(float modifier in provider.GetAddetiveMidifier(stat))
            {
               total += modifier;
            }
         }
         return total;
      }
      public float GetStat(Stat stat)
      {
         return progression.GetStat(stat, characterClass, GetLevel()) + GetAdditionalModifier(stat);
      }

      public int GetLevel()
      {
         return currentLevel;
      }
   }
}
