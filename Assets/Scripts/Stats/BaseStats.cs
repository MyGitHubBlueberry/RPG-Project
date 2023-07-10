using System;
using UnityEngine;
using static RPG.Stats.IModifierProvider;

namespace RPG.Stats
{
   public class BaseStats : MonoBehaviour
   {
      [Range(1,99)]
      [SerializeField] private int startingLevel = 1;
      [SerializeField] private CharacterClass characterClass;
      [SerializeField] private Progression progression;
      [SerializeField] private GameObject levelUpParticleEffect;
      [SerializeField] private bool shouldUseModifiers;
      
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
      
      private float GetModifiers(Stat stat, Modifier modifier)
      {
         if(!shouldUseModifiers) return 0;
         
         float total = 0;
         foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
         {
            foreach(float modifierValue in provider.GetModifiers(stat, modifier))
            {
               total += modifierValue;
            }
         }
         return total;
      }

      private float GetBaseStat(Stat stat)
      {
         return progression.GetStat(stat, characterClass, GetLevel());
      }

      public float GetStat(Stat stat)
      {
         return (GetBaseStat(stat) + GetModifiers(stat, Modifier.Additive)) * (1 + GetModifiers(stat, Modifier.Persantage)/100);
      }

      public int GetLevel()
      {
         return currentLevel;
      }
   }
}
