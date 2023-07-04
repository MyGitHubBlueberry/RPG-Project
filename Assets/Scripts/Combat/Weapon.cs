using System;
using UnityEngine;

namespace RPG.Combat
{    
   [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
   public class Weapon : ScriptableObject
   {


      [SerializeField] private AnimatorOverrideController animatorOverride = null;
      [SerializeField] private GameObject weaponPrefab = null;

      public void Spawn(Transform hand, out AnimatorOverrideController animatorOverride)
      {
         animatorOverride = this.animatorOverride;
         Instantiate(weaponPrefab, hand);
      }
   }
}