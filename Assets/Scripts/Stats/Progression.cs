using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Stats
{
   [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression")]
   public class Progression : ScriptableObject
   {
      [SerializeField] private ProgressionCharacterClass[] characterClasses;

      [Serializable]
      class ProgressionCharacterClass
      {
         [SerializeField] CharacterClass characterClass;
         [SerializeField] private float[] health;
      }
   }
}
