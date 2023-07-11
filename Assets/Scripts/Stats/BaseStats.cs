using System;
using GameDevTV.Utils;
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
      public event Action OnRestoreLevel;

      private Experience experience;
      private LazyValue<int> currentLevel;

      private void Awake()
      {
         experience = GetComponent<Experience>();
         currentLevel = new LazyValue<int>(CalculateLevel);
      }

      private void OnEnable()
      {
         if(experience != null)
         {
            experience.OnExperienceGained += GainLevel;
            experience.OnExperienceRestored += RestoreLevel;
         }
      }

      private void Start()
      {
         currentLevel.ForceInit();
      }
      private void OnDisable()
      {
         if(experience != null)
         {
            experience.OnExperienceGained -= GainLevel;
            experience.OnExperienceRestored -= RestoreLevel;
         }
      }

      private int CalculateLevel()
      {
         if(experience == null) return startingLevel;

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

      private void GainLevel()
      {
         if(UpdateLevel()) 
         {
            OnLevelUp?.Invoke();
            InstantiateLevelUpEffect();
         }
      }

      private void RestoreLevel()
      {
         UpdateLevel();
         OnRestoreLevel?.Invoke();
      }
      private bool UpdateLevel()
      {
         int newLevel = CalculateLevel();
         if(newLevel > currentLevel.value)
         {
            currentLevel.value = newLevel;
            return true;
         }
         return false;
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
         return currentLevel.value;
      }
   }
}
