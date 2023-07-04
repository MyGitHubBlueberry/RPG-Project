using System;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
   public class Fighter : MonoBehaviour, IAction
   {
      public event EventHandler OnAttack;
      public event EventHandler OnAttackCanceled;
      public event EventHandler<OnAnyWeaponSpawnedEventArgs> OnWeaponSpawned;
      public class OnAnyWeaponSpawnedEventArgs : EventArgs
      {
         public AnimatorOverrideController AnimatorOverride { get; set; }
      }

      [SerializeField] private float timeBetweenAttacks = 1f;
      [SerializeField] private Transform hand = null;
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
         EquipWeapon(defaultWeapon);
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

      public void EquipWeapon(Weapon weapon)
      {
         currentWeapon = weapon;
         weapon.Spawn(hand, out AnimatorOverrideController animatorOverride);

         OnWeaponSpawned?.Invoke(this, new OnAnyWeaponSpawnedEventArgs{
            AnimatorOverride = animatorOverride,
         });
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

       //*AnimationEvent
      private void Hit()
      {
         if(target == null) return;
         target.TakeDamage(currentWeapon.GetDamage());
      }

      private bool GetIsInRange()
      {
         return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
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
   }
}