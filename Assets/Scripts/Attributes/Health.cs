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
      [SerializeField] private float regenerationPercentage = 70;

      public event EventHandler OnZeroHealth;

      private float health = -1f;
      private bool isDead;

      private void Start()
      {
         GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;

         if(health < 0)
         {
            health = GetComponent<BaseStats>().GetStat(Stat.Health);
         }
      }

      private void RegenerateHealth()
      {
         float regenHealthPoints = GetMaxHealth() * regenerationPercentage / 100;
         health = (GetPercentage() > regenerationPercentage) ? health : regenHealthPoints;
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

      private void AwardExperience(GameObject instigator)
      {
         Experience experience = instigator.GetComponent<Experience>();
         
         if(experience == null) return;

         float rewardXP = GetComponent<BaseStats>().GetStat(Stat.ExperienceReward);
         experience.GainExperience(rewardXP);
      }

      public void TakeDamage(GameObject instigator,float damage)
      {
         //TODO remove
         print(gameObject.name + " took damage: " + damage);
         //TODO remove
         health = Mathf.Max(health - damage, 0f);
         if(health == 0)
         {
            Die();
            AwardExperience(instigator);
         }
      }

      public bool GetIsDead()
      {
         return isDead;
      }

      public float GetPercentage()
      {
         return health / GetMaxHealth() * 100;
      }

      public float GetMaxHealth()
      {
         return GetComponent<BaseStats>().GetStat(Stat.Health);
      }

      public float GetHealth()
      {
         return health;
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
