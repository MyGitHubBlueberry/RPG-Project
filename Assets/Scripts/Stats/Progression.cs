using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
   [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression")]
   public class Progression : ScriptableObject
   {
      [SerializeField] private ProgressionCharacterClass[] characterClasses;
      
      public float GetStat(Stat stat,CharacterClass characterClass, int level)
      {
         List<ProgressionCharacterClass> progressionCharacterClasses = characterClasses.ToList();

         var statValue = from progressionCharacterClass in progressionCharacterClasses
                           where progressionCharacterClass.characterClass == characterClass
                           select progressionCharacterClass.stats into progressionStats
                         from progressionStat in progressionStats
                           where progressionStat.stat == stat
                           where progressionStat.levels.Length >= level
                           select progressionStat.levels[level - 1];
         
      

         return statValue.First();
      }

      [Serializable]
      class ProgressionCharacterClass
      {
         public CharacterClass characterClass;
         public ProgressionStat[] stats;
      }
      [Serializable]
      public class ProgressionStat
      {
         public Stat stat;
         public float[] levels;
      }
   }
}
