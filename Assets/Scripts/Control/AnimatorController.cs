using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;

namespace RPG.Control
{
   public class AnimatorController : MonoBehaviour
   {
      private Animator animator;
      private Mover mover;
      private Fighter fighter;
      private Health health;
      private bool isDead;
      private const string FORWARD_SPEED = "forwardSpeed";
      private const string ATTACK = "attack";
      private const string DIE = "die";
      private const string CANCEL_ATTACK = "cancelAttack";


      private void Awake()
      {
         animator = GetComponent<Animator>();
         mover = GetComponent<Mover>();
         fighter = GetComponent<Fighter>();
         health = GetComponent<Health>();
      }
      private void OnEnable()
      {
         fighter.OnWeaponSpawned += Fighter_OnWeaponSpawned;
         health.OnZeroHealth += Health_OnZeroHealth;
         fighter.OnAttack += Fighter_OnAttack;
         fighter.OnAttackCanceled += Fighter_OnAttackCanceled;
      }

      private void Update()
      {
         if(health.GetIsDead()) return;

         UpdateMovement(mover.GetMovementSpeed());      
      }

      private void OnDisable()
      {
         fighter.OnWeaponSpawned -= Fighter_OnWeaponSpawned;
         health.OnZeroHealth -= Health_OnZeroHealth;
         fighter.OnAttack -= Fighter_OnAttack;
         fighter.OnAttackCanceled -= Fighter_OnAttackCanceled;
      }
      
      private void Fighter_OnWeaponSpawned(object sender, Fighter.OnAnyWeaponSpawnedEventArgs e)
      {
         if (e.AnimatorOverride != null)
         {
            animator.runtimeAnimatorController = e.AnimatorOverride;
         }
         else if (IsAnimatorOverridden(out AnimatorOverrideController animatorOverride))
         {
            animator.runtimeAnimatorController = animatorOverride.runtimeAnimatorController;
         }
      }

      private bool IsAnimatorOverridden(out AnimatorOverrideController animatorOverride)
      {
         animatorOverride = animator.runtimeAnimatorController as AnimatorOverrideController;
         return animatorOverride != null;
      }

      private void Fighter_OnAttackCanceled()
      {
         animator.ResetTrigger(ATTACK);
         animator.SetTrigger(CANCEL_ATTACK);
      }

      private void Health_OnZeroHealth()
      {
         animator.ResetTrigger(DIE);
         animator.SetTrigger(DIE);
      }

      private void Fighter_OnAttack()
      {
         animator.ResetTrigger(CANCEL_ATTACK);
         animator.SetTrigger(ATTACK);
      }

      private void UpdateMovement(float movementSpeed)
      {
         animator.SetFloat(FORWARD_SPEED, movementSpeed);
      }
   }
}