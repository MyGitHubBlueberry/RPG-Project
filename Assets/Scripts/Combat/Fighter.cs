using System;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
   public class Fighter : MonoBehaviour, IAction
   {
      public event EventHandler OnAttack;

      [SerializeField] private float weaponRange = 2f;
      [SerializeField] private float timeBetweenAttacks = 1f;

      private Transform target;
      private Mover mover;
      private float timeSinceLastAttack;


      private void Start()
      {
         mover = GetComponent<Mover>();
      }

      private void Update()
      {
         timeSinceLastAttack += Time.deltaTime;

         if(target == null) return;

         if (!GetIsInRange())
         {
            mover.MoveTo(target.position);
         }
         else
         {
            mover.Cancel();
            AttackBehaviour();
         }
      }

      private void AttackBehaviour()
      {
         if(timeSinceLastAttack > timeBetweenAttacks)
         {
            OnAttack?.Invoke(this, EventArgs.Empty);
            timeSinceLastAttack = 0f;
         }
      }

      private bool GetIsInRange()
      {
         return Vector3.Distance(transform.position, target.position) < weaponRange;
      }

      public void Attack(CombatTarget combatTarget)
      {
         GetComponent<ActionScheduler>().StartAction(this);
         target = combatTarget.transform;
      }

      //*AnimationEvent
      private void Hit()
      {

      }

      public void Cancel()
      {
         target = null;
      }
   }
}