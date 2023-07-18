using System;
using UnityEngine;
using GameDevTV.Utils;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using RPG.Core;
using RPG.Stats;
using RPG.Saving;
using RPG.Movement;
using RPG.Animation;
using RPG.Attributes;
using RPG.SFX;

namespace RPG.Combat
{
   public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider, 
                          IAnimationTriggerEvent, IOverrideRuntimeAnimatorControllerEvent, 
                          ISFXEvent
   {
      public event EventHandler<IAnimationTriggerEvent.OnResetSetAnimationTriggerRequestEventArgs> OnResetSetAnimationTriggerRequest;
      public event Action<AnimatorOverrideController> OnOverrideRuntimeAnimatorControllerRequest;
      public event Action<SFXParameter> OnSFXTriggerRequest;
      public event Action OnAttackCanceled;
      public event Action OnTargetSet;

      [SerializeField] private float timeBetweenAttacks = 1f;
      [SerializeField] private Transform rightHand = null;
      [SerializeField] private Transform leftHand = null;
      [SerializeField] private WeaponConfig defaultWeaponConfig;

      private Health target;
      private Mover mover;
      private WeaponConfig currentWeaponConfig;
      private LazyValue<Weapon> currentWeapon;
      private float timeSinceLastAttack = Mathf.Infinity;

      private void Awake()
      {
         mover = GetComponent<Mover>();
         currentWeaponConfig = defaultWeaponConfig;
         currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
      }

      private Weapon SetupDefaultWeapon()
      {
         return AttachWeapon(defaultWeaponConfig);
      }

      private void Start()
      {
         currentWeapon.ForceInit();
      }

      private void Update()
      {
         timeSinceLastAttack += Time.deltaTime;

         if (target == null || target.IsDead()) return;

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
            //*Listener triggers Hit() or Shoot() event
            OnResetSetAnimationTriggerRequest?.Invoke(this, new IAnimationTriggerEvent.OnResetSetAnimationTriggerRequestEventArgs
            {
               resetTrigger = AnimatorParameters.Trigger.cancelAttack,
               setTrigger = AnimatorParameters.Trigger.attack,
            });
            timeSinceLastAttack = 0f;
         }
      }

#region Animation events
      private void Hit()
      {
         if(target == null) return;

         float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

         OnSFXTriggerRequest?.Invoke(currentWeaponConfig.GetSFXParameter());

         if(currentWeapon.value != null)
         {
            currentWeapon.value.OnHit();
         }
         
         if(currentWeaponConfig.HasProjectile())
         {
            currentWeaponConfig.LaunchProjectile(rightHand, leftHand, target, gameObject, damage);
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
         return Vector3.Distance(transform.position, target.transform.position) < currentWeaponConfig.GetRange();
      }

      public IEnumerable<float> GetAdditiveModifiers(Stat stat)
      {
         if(stat == Stat.Damage)
         {
            yield return currentWeaponConfig.GetDamage();
         }
      }

      public IEnumerable<float> GetPersantageModifiers(Stat stat)
      {
         if(stat == Stat.Damage)
         {
            yield return currentWeaponConfig.GetPersantageBonus();
         }
      }
      
      public void EquipWeapon(WeaponConfig weaponConfig)
      {
         currentWeaponConfig = weaponConfig;
         currentWeapon.value = AttachWeapon(weaponConfig);
      }

      private Weapon AttachWeapon(WeaponConfig weaponConfig)
      {
         Weapon weapon = weaponConfig.Spawn(rightHand, leftHand, out AnimatorOverrideController animatorOverride);

         OnOverrideRuntimeAnimatorControllerRequest?.Invoke(animatorOverride);

         return weapon;
      }

      public bool CanAttack(GameObject combatTarget)
      {
         return combatTarget.GetComponent<Health>() != null && combatTarget.GetComponent<Health>().IsAlive();
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
         OnResetSetAnimationTriggerRequest?.Invoke(this, new IAnimationTriggerEvent.OnResetSetAnimationTriggerRequestEventArgs
         {
            resetTrigger = AnimatorParameters.Trigger.attack,
            setTrigger = AnimatorParameters.Trigger.cancelAttack,
         });
         mover.Cancel();
      }

      public JToken CaptureAsJToken()
      {
         return JToken.FromObject(currentWeaponConfig.name);
      }

      public void RestoreFromJToken(JToken state)
      {
         string weaponName = state.ToObject<string>();
         WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
         EquipWeapon(weapon);
      }

      public Health GetTarget()
      {
         return target;
      }

   }
}