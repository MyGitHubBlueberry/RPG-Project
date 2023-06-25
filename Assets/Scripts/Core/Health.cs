using System;
using UnityEngine;

namespace RPG.Core
{
   public class Health : MonoBehaviour
   {
      public event EventHandler OnZeroHealth;

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
         OnZeroHealth?.Invoke(this, EventArgs.Empty);
      }

      public bool GetIsDead()
      {
         return isDead;
      }
   }
}
