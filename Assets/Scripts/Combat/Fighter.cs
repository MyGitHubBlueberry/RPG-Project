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

      private Transform target;
      private Mover mover;

      private void Start()
      {
         mover = GetComponent<Mover>();
      }

      private void Update()
      {
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
         OnAttack?.Invoke(this, EventArgs.Empty);
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