using System;
using RPG.Core;
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
      [SerializeField] private Projectile projectile = null;
 
      public void Spawn(Transform rightHand, Transform leftHand, out AnimatorOverrideController animatorOverride)
      {
         animatorOverride = this.animatorOverride;
         
         if(equippedPrefab != null)
         {
            Transform hand = GetHand(rightHand, leftHand);
            Instantiate(equippedPrefab, hand);
         }
      }

      private Transform GetHand(Transform rightHand, Transform leftHand)
      {
         return isRightHanded ? rightHand : leftHand;
      }

      public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
      {
         Projectile projectileInstance = Instantiate(projectile,GetHand(rightHand, leftHand).position, Quaternion.identity);
         projectileInstance.SetTarget(target);
      }

      public bool HasProjectile()
      {
         return projectile != null;
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