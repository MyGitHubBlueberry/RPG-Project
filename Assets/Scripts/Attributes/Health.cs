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

      private void Die()
      {
         if(isDead) return;

         isDead = true;
         GetComponent<ActionScheduler>().CancelCurrentAction();
         OnZeroHealth?.Invoke(this, EventArgs.Empty);
      }

      private void UpdateState()
      {
         if(health <= 0)
         {
            Die();
         }
      }

      public void TakeDamage(GameObject instigator,float damage)
      {
         health = Mathf.Max(health - damage, 0f);
         if(health == 0)
         {
            Die();
            AwardExperience(instigator);
         }
      }

      private void AwardExperience(GameObject instigator)
      {
         Experience experience = instigator.GetComponent<Experience>();
         
         if(experience == null) return;

         float rewardXP = GetComponent<BaseStats>().GetExperienceReward();
         experience.GainExperience(rewardXP);
      }

      public bool GetIsDead()
      {
         return isDead;
      }

      public float GetPercentage()
      {
         return health / GetComponent<BaseStats>().GetHealth()  * 100;
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
   }
}
