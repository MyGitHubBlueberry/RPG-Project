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
                     progressionCharacterClass.GetCharacterClass() == characterClass);

         return progression.GetHealthByLevel(level);
      }

      [Serializable]
      class ProgressionCharacterClass
      {
         [SerializeField] private CharacterClass characterClass;
         [SerializeField] private float[] health;

         public CharacterClass GetCharacterClass()
         {
            return characterClass;
         }

         public float GetHealthByLevel(int level)
         {
            int index = Mathf.Max(level - 1, 0);
            return health[index];
         }
      }
   }
}
