using RPG.Attributes;
using RPG.SFX;
using UnityEngine;

namespace RPG.Combat
{    
   [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
   public class WeaponConfig : ScriptableObject
   {
      private const string WEAPON_NAME = "Weapon";
      [SerializeField] private AnimatorOverrideController animatorOverride = null;
      [SerializeField] private Weapon equippedPrefab = null;
      [SerializeField] private float damage = 5f;
      [SerializeField] private float persantageBonus = 0f;
      [SerializeField] private float range = 2f;
      [SerializeField] private bool isRightHanded = true;
      [SerializeField] private Projectile projectile = null;
 
      public Weapon Spawn(Transform rightHand, Transform leftHand, out AnimatorOverrideController animatorOverride)
      {
         animatorOverride = this.animatorOverride;

         DestroyOldWeapon(rightHand, leftHand);
         
         Weapon weapon = null;

         if(equippedPrefab != null)
         {
            Transform hand = GetHand(rightHand, leftHand);
            weapon = Instantiate(equippedPrefab, hand);
            weapon.gameObject.name = WEAPON_NAME;

            return weapon;
         }

         return weapon;
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

      public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
      {
         Projectile projectileInstance = Instantiate(projectile,GetHand(rightHand, leftHand).position, Quaternion.identity);
         projectileInstance.SetTarget(target, instigator, calculatedDamage);
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

      public float GetPersantageBonus()
      {
         return persantageBonus;
      }
   }
}