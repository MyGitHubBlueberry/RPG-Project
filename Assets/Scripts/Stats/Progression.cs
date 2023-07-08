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
      
      public float GetHealth(CharacterClass characterClass, int level)
      {
         List<ProgressionCharacterClass> progressionCharacterClasses = characterClasses.ToList();

         var progression = progressionCharacterClasses.
               First(progressionCharacterClass => 
                     progressionCharacterClass.characterClass == characterClass);

         // return progression.health[level -1];
         return 0;
      }

      [Serializable]
      class ProgressionCharacterClass
      {
         public CharacterClass characterClass;
         public ProgressionStat[] stats;
         //public float[] health;
      }
      [Serializable]
      public class ProgressionStat
      {
         public Stat stat;
         public float[] levels;
      }
   }
}
