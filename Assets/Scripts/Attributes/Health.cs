using System;
using UnityEngine;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

namespace RPG.Attributes
{
   public class Health : MonoBehaviour, ISaveable
   {
      public event EventHandler OnZeroHealth;

      [SerializeField] private float health = 100f;

      private bool isDead;

      private void OnEnable()
      {
         health = GetComponent<BaseStats>().GetHealth();
      }

      public void TakeDamage(float damage)
      {
         health = Mathf.Max(health - damage, 0f);
         if(health == 0)
         {
            Die();
         }
      }

      private void Die()
      {
         if(isDead) return;

         isDead = true;
         GetComponent<ActionScheduler>().CancelCurrentAction();
         OnZeroHealth?.Invoke(this, EventArgs.Empty);
      }

      public bool GetIsDead()
      {
         return isDead;
      }

      public JToken CaptureAsJToken()
      {
         return JToken.FromObject(health);
      }

      public void RestoreFromJToken(JToken state)
      {
         health = state.ToObject<float>();
         UpdateState();
      }

      private void UpdateState()
      {
         if(health <= 0)
         {
            Die();
         }
      }

   }
}
