using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using MyBox;

namespace RPG.Stats
{
   [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression")]
   public class Progression : ScriptableObject
   {
      private const int PENULTIMATE_LEVEL = 99;

      [SerializeField] private CharacterClass characterClass;
      [SerializeField] private bool isDependsOnProgression;

      [ConditionalField(nameof(isDependsOnProgression))]
      [SerializeField] private Progression baseProgression;
      [SerializeField] private ProgressionStat[] stats;

      [Serializable]
      private class ProgressionStat
      {
         public Stat stat;
         public bool isDependsOnProgression;

         [ConditionalField(nameof(isDependsOnProgression), true)]
         public StatFormula formula;

         [ConditionalField(nameof(isDependsOnProgression))]
         public float statMultiplier = 1;
      }

      [Serializable]
      private class StatFormula
      {
         [SerializeField] float level1 = 10;
         [SerializeField] float level100 = 500;

         float deltaPerLevel;

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
         public void GetLevels(out float level1, out float level100)
         {
            level1 = this.level1;
            level100 = this.level100;
         }

         public void SetLevels(float level1, float level100, float multiplier)
         {
            this.level1 = level1 * multiplier; 
            this.level100 = level100 * multiplier;
         }
      }

      public float GetStatEvaluation(Stat stat, int level)
      {
         if(isDependsOnProgression)
         {
            SetLevels(stat);
         }

         var statEvaluation = GetProgressionStat(this, stat).formula.EvaluateAsInt(level);

         return statEvaluation;
      }

      private void SetLevels(Stat stat)
      {
         if (baseProgression == null)
         {
            Debug.LogError("baseProgression can not be null");
         }
         var currentProgressionStat = GetProgressionStat(this, stat);

         if(!currentProgressionStat.isDependsOnProgression) return;

         var baseStatFormula = GetProgressionStat(baseProgression, stat).formula;
         baseStatFormula.GetLevels(out float level1, out float level100);

         currentProgressionStat.formula.SetLevels(level1, level100, currentProgressionStat.statMultiplier);
      }

      private ProgressionStat GetProgressionStat(Progression progression, Stat stat)
      {
         var output = from progressionStat in progression.stats 
                      where progressionStat.stat == stat
                      select progressionStat;
         return output.First();
      }

      public int GetPenultimateLevel()
      {
         return PENULTIMATE_LEVEL;
      }
   }
}
