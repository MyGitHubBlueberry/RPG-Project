using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using System;

namespace RPG.Control
{
   public class AnimatorController : MonoBehaviour
   {
      private Animator animator;
      private Mover mover;
      private Fighter fighter;
      private Health health;
      private const string FORWARD_SPEED = "forwardSpeed";
      private const string ATTACK = "attack";
      private const string DIE = "die";
      private const string CANCEL_ATTACK = "cancelAttack";

      //State name
      private const string DEATH = "Death";


      private void Awake()
      {
         animator = GetComponent<Animator>();
         mover = GetComponent<Mover>();
         fighter = GetComponent<Fighter>();
         health = GetComponent<Health>();
      }
      private void OnEnable()
      {
         fighter.OnWeaponSpawned += fighter_OnWeaponSpawned;
      }

      private void Start()
      {
         fighter.OnAttack += Fighter_OnAttack;
         fighter.OnAttackCanceled += fighter_OnAttackCanceled;
         health.OnZeroHealth += Health_OnZeroHealth;
         health.OnLoadedDead += Health_OnLoadedDead;
      }

      private void fighter_OnWeaponSpawned(object sender, Fighter.OnAnyWeaponSpawnedEventArgs e)
      {
         if(e.AnimatorOverride == null) return;
         animator.runtimeAnimatorController = e.AnimatorOverride;
      }

      private void Health_OnLoadedDead(object sender, EventArgs e)
      {
         animator.Play(DEATH);
      }

      private void fighter_OnAttackCanceled(object sender, EventArgs e)
      {
         animator.ResetTrigger(ATTACK);
         animator.SetTrigger(CANCEL_ATTACK);
      }

      private void Health_OnZeroHealth(object sender, EventArgs e)
      {
         animator.SetTrigger(DIE);
      }

      private void Fighter_OnAttack(object sender, EventArgs e)
      {
         animator.ResetTrigger(CANCEL_ATTACK);
         animator.SetTrigger(ATTACK);
      }

      private void Update()
      {
         UpdateMovement(mover.GetMovementSpeed());      
      }

      private void UpdateMovement(float movementSpeed)
      {
         animator.SetFloat(FORWARD_SPEED, movementSpeed);
      }
   }
}