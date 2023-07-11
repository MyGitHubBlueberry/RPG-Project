using System;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using Newtonsoft.Json.Linq;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat
{
   public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
   {
      public event Action OnTargetSet;
      public event Action OnAttack;
      public event Action OnAttackCanceled;
      public event EventHandler<OnAnyWeaponSpawnedEventArgs> OnWeaponSpawned;
      public class OnAnyWeaponSpawnedEventArgs : EventArgs
      {
         public AnimatorOverrideController AnimatorOverride { get; set; }
      }

      [SerializeField] private float timeBetweenAttacks = 1f;
      [SerializeField] private Transform rightHand = null;
      [SerializeField] private Transform leftHand = null;
      [SerializeField] private Weapon defaultWeapon;


      private Health target;
      private Mover mover;
      private LazyValue<Weapon> currentWeapon;
      private float timeSinceLastAttack = Mathf.Infinity;


      private void Awake()
      {
         mover = GetComponent<Mover>();
         currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
      }

      private Weapon SetupDefaultWeapon()
      {
         AttachWeapon(defaultWeapon);
         return defaultWeapon;
      }

      private void Start()
      {
         currentWeapon.ForceInit();
      }

      private void Update()
      {
         timeSinceLastAttack += Time.deltaTime;

         if (target == null || target.GetIsDead()) return;

         if (!GetIsInRange())
         {
            mover.MoveTo(target.transform.position, 1f);
         }
         else
         {
            mover.Cancel();
            AttackBehaviour();
         }
      }

      private void AttackBehaviour()
      {
         transform.LookAt(target.transform);
         if(timeSinceLastAttack > timeBetweenAttacks)
         {
            //*Listener triggers Hit() event
            OnAttack?.Invoke();
            timeSinceLastAttack = 0f;
         }
      }

#region Animation events
      private void Hit()
      {
         if(target == null) return;

         float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
         if(currentWeapon.value.HasProjectile())
         {
            currentWeapon.value.LaunchProjectile(rightHand, leftHand, target, gameObject, damage);
         }
         else
         {
            target.TakeDamage(gameObject, damage);
         }
      }

      private void Shoot()
      {
         Hit();
      }
#endregion

      private bool GetIsInRange()
      {
         return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.value.GetRange();
      }

      public IEnumerable<float> GetAdditiveModifiers(Stat stat)
      {
         if(stat == Stat.Damage)
         {
            yield return currentWeapon.value.GetDamage();
         }
      }

      public IEnumerable<float> GetPersantageModifiers(Stat stat)
      {
         if(stat == Stat.Damage)
         {
            yield return currentWeapon.value.GetPersantageBonus();
         }
      }
      
      public void EquipWeapon(Weapon weapon)
      {
         currentWeapon.value = weapon;
         AttachWeapon(weapon);
      }

      private void AttachWeapon(Weapon weapon)
      {
         weapon.Spawn(rightHand, leftHand, out AnimatorOverrideController animatorOverride);

         OnWeaponSpawned?.Invoke(this, new OnAnyWeaponSpawnedEventArgs
         {
            AnimatorOverride = animatorOverride,
         });
      }

      public bool CanAttack(GameObject combatTarget)
      {
         return combatTarget.GetComponent<Health>() != null && !combatTarget.GetComponent<Health>().GetIsDead();
      }

      public void Attack(GameObject combatTarget)
      {
         GetComponent<ActionScheduler>().StartAction(this);
         target = combatTarget.GetComponent<Health>();
         OnTargetSet?.Invoke();
      }

      public void Cancel()
      {
         target = null;
         OnAttackCanceled?.Invoke();
         mover.Cancel();
      }

      public JToken CaptureAsJToken()
      {
         return JToken.FromObject(currentWeapon.value.name);
      }

      public void RestoreFromJToken(JToken state)
      {
         string weaponName = state.ToObject<string>();
         Weapon weapon = Resources.Load<Weapon>(weaponName);
         EquipWeapon(weapon);
      }

      public Health GetTarget()
      {
         return target;
      }

   }
}