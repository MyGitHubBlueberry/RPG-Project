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

      [SerializeField] private float weaponRange = 2f;
      [SerializeField] private float weaponDamage = 5f;
      [SerializeField] private float timeBetweenAttacks = 1f;

      private Health target;
      private Mover mover;
      private float timeSinceLastAttack;


      private void Start()
      {
         mover = GetComponent<Mover>();
      }

      private void Update()
      {
         timeSinceLastAttack += Time.deltaTime;

         if(target == null || target.GetIsDead()) return;

         if (!GetIsInRange())
         {
            mover.MoveTo(target.transform.position);
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

       //*AnimationEvent
      private void Hit()
      {
         target.TakeDamage(weaponDamage);
      }

      private bool GetIsInRange()
      {
         return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
      }

      public void Attack(CombatTarget combatTarget)
      {
         GetComponent<ActionScheduler>().StartAction(this);
         target = combatTarget.GetComponent<Health>();
      }

      public void Cancel()
      {
         OnAttackCanceled?.Invoke(this, EventArgs.Empty);
         target = null;
      }
   }
}