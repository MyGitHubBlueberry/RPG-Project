using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{    
   [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
   public class Weapon : ScriptableObject
   {
      private const string WEAPON_NAME = "Weapon";
      [SerializeField] private AnimatorOverrideController animatorOverride = null;
      [SerializeField] private GameObject equippedPrefab = null;
      [SerializeField] private float damage = 5f;
      [SerializeField] private float range = 2f;
      [SerializeField] private bool isRightHanded = true;
      [SerializeField] private Projectile projectile = null;
 
      public void Spawn(Transform rightHand, Transform leftHand, out AnimatorOverrideController animatorOverride)
      {
         animatorOverride = this.animatorOverride;

         DestroyOldWeapon(rightHand, leftHand);
         
         if(equippedPrefab != null)
         {
            Transform hand = GetHand(rightHand, leftHand);
            GameObject weapon = Instantiate(equippedPrefab, hand);
            weapon.name = WEAPON_NAME;
         }
      }

      private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
      {
         Transform oldWeapon = rightHand.Find(WEAPON_NAME) ?? leftHand.Find(WEAPON_NAME);

         if(oldWeapon is null) return;

         oldWeapon.name = "DESTROYING";
         Destroy(oldWeapon.gameObject);
      }

      private Transform GetHand(Transform rightHand, Transform leftHand)
      {
         return isRightHanded ? rightHand : leftHand;
      }

      public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
      {
         Projectile projectileInstance = Instantiate(projectile,GetHand(rightHand, leftHand).position, Quaternion.identity);
         projectileInstance.SetTarget(target, damage);
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