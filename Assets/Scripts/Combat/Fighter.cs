using System;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using Newtonsoft.Json.Linq;
using RPG.Attributes;
using RPG.Stats;

namespace RPG.Combat
{
   public class Fighter : MonoBehaviour, IAction, ISaveable
   {
      public event EventHandler OnAttack;
      public event EventHandler OnAttackCanceled;
      public event EventHandler<OnAnyWeaponSpawnedEventArgs> OnWeaponSpawned;
      public class OnAnyWeaponSpawnedEventArgs : EventArgs
      {
         public AnimatorOverrideController AnimatorOverride { get; set; }
      }

      [SerializeField] private float timeBetweenAttacks = 1f;
      [SerializeField] private Transform rightHand = null;
      [SerializeField] private Transform leftHand = null;
      [SerializeField] private Weapon defaultWeapon = null;


      private Health target;
      private Mover mover;
      private Weapon currentWeapon = null;
      private float timeSinceLastAttack = Mathf.Infinity;


      private void Awake()
      {
         mover = GetComponent<Mover>();
      }

      private void Start()
      {
         if(currentWeapon == null)
         {
            EquipWeapon(defaultWeapon);
         }
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
            OnAttack?.Invoke(this, EventArgs.Empty);
            timeSinceLastAttack = 0f;
         }
      }

      #region Animation events
      private void Hit()
      {
         if(target == null) return;

         float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
         if(currentWeapon.HasProjectile())
         {
            currentWeapon.LaunchProjectile(rightHand, leftHand, target, gameObject, damage);
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
         return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
      }
      
      public void EquipWeapon(Weapon weapon)
      {
         currentWeapon = weapon;
         weapon.Spawn(rightHand, leftHand, out AnimatorOverrideController animatorOverride);

         OnWeaponSpawned?.Invoke(this, new OnAnyWeaponSpawnedEventArgs{
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
      }

      public void Cancel()
      {
         OnAttackCanceled?.Invoke(this, EventArgs.Empty);
         target = null;
         mover.Cancel();
      }

      public JToken CaptureAsJToken()
      {
         return JToken.FromObject(currentWeapon.name);
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