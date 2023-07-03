using System;
using UnityEngine;
using Newtonsoft.Json.Linq;
using RPG.Saving;

namespace RPG.Core
{
   public class Health : MonoBehaviour, ISaveable
   {
      public event EventHandler OnZeroHealth;
      public event EventHandler OnLoadedDead;

      [SerializeField] private float health = 100f;

      private bool isDead;

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
            OnLoadedDead?.Invoke(this, EventArgs.Empty);
         }
      }

   }
}
