using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RPG.Stats
{
   [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression")]
   public class Progression : ScriptableObject
   {
      private const int PENULTIMATE_LEVEL = 99;

      [SerializeField] private CharacterClass characterClass;
      [SerializeField] private ProgressionStat[] stats;

      [ExecuteAlways]
      [Serializable]
      private class ProgressionStat
      {
         public Stat stat;
         public StatFormula formula;
      }

      [Serializable]
      private class StatFormula
      {
         [SerializeField] float level1 = 10;
         [SerializeField] float level100 = 500;

         float deltaPerLevel;
         bool isDeltaCalculated;

         private void CalculateDelta()
         {
            deltaPerLevel = deltaPerLevel = (level100 - level1) / 99;
         }

         public float Evaluate(int level)
         {
            CalculateDelta();

            level--;
            if(level<0) level = 0;
            return level1 + deltaPerLevel * level;
         }
         public int EvaluateAsInt(int level) => Mathf.RoundToInt(Evaluate(level));
      }

      public float GetStatEvaluation(Stat stat, int level)
      {
         var statEvaluation = from progressionStat in stats 
                              where progressionStat.stat == stat
                              select progressionStat.formula.EvaluateAsInt(level);

         return statEvaluation.First();
      }

      public int GetPenultimateLevel()
      {
         return PENULTIMATE_LEVEL;
      }
   }
}
