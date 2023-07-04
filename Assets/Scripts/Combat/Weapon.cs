using System;
using UnityEngine;

namespace RPG.Combat
{    
   [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
   public class Weapon : ScriptableObject
   {
      [SerializeField] private AnimatorOverrideController animatorOverride = null;
      [SerializeField] private GameObject equippedPrefab = null;
      [SerializeField] private float damage = 5f;
      [SerializeField] private float range = 2f;
      [SerializeField] private bool isRightHanded = true;


      public void Spawn(Transform rightHand, Transform leftHand, out AnimatorOverrideController animatorOverride)
      {
         animatorOverride = this.animatorOverride;
         
         if(equippedPrefab != null)
         {
            Transform hand = isRightHanded ? rightHand : leftHand;
            Instantiate(equippedPrefab, hand);
         }
      }

      public float GetRange()
      {
         return range;
      }
      
      public float GetDamage()
      {
         return damage;
      }
   }
}