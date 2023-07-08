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

      Dictionary<CharacterClass, Dictionary<Stat,float[]>> lookupTable;
      
      public float GetStat(Stat stat,CharacterClass characterClass, int level)
      {
         BuildLookup();

         float[] levels = lookupTable[characterClass][stat];

         if(levels.Length < level) return 0;

         return levels[level - 1];
      }

      private void BuildLookup()
      {
         if(lookupTable != null) return;

         lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

         foreach(ProgressionCharacterClass progressionClass in characterClasses)
         {
            Dictionary<Stat, float[]> statLookupTable = new Dictionary<Stat, float[]>();

            foreach(ProgressionStat progressionStat in progressionClass.stats)
            {
               statLookupTable.Add(progressionStat.stat, progressionStat.levels);
            }

            lookupTable.Add(progressionClass.characterClass, statLookupTable);
         }
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
