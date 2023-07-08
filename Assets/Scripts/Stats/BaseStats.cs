using UnityEngine;

namespace RPG.Stats
{
   public class BaseStats : MonoBehaviour
   {
      [Range(1,99)]
      [SerializeField] private int level = 1;
      [SerializeField] private CharacterClass characterClass;
      [SerializeField] private Progression progression;

      public float GetStat(Stat stat)
      {
         return progression.GetStat(stat ,characterClass, level);
      }
   }
}
