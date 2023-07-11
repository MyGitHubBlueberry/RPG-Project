using System;
using UnityEngine;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using GameDevTV.Utils;

namespace RPG.Attributes
{
   public class Health : MonoBehaviour, ISaveable
   {
      [SerializeField] private float regenerationPercentage = 70;

      public event Action OnZeroHealth;
      public event Action OnHealthChanged;

      private LazyValue<float> health;
      private bool isDead;
      
      private void Awake()
      {
         health = new LazyValue<float>(GetInitialHealth);
      }
      private void OnEnable()
      {
         GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
      }

      private void Start() => health.ForceInit();

      private void OnDisable()
      {
         GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;
      }
      
      private float GetInitialHealth()
      {
         return GetComponent<BaseStats>().GetStat(Stat.Health);
      }

      private void RegenerateHealth()
      {
         float regenHealthPoints = GetMaxHealth() * regenerationPercentage / 100;
         health.value = (GetPercentage() > regenerationPercentage) ? health.value : regenHealthPoints;

         OnHealthChanged?.Invoke();
      }

      private void Die()
      {
         if(isDead) return;

         isDead = true;
         GetComponent<ActionScheduler>().CancelCurrentAction();
         OnZeroHealth?.Invoke();
      }

      private void UpdateState()
      {
         if(health.value <= 0)
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
         health.value = Mathf.Max(health.value - damage, 0f);
         
         OnHealthChanged?.Invoke();

         if(health.value == 0)
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
         return health.value / GetMaxHealth() * 100;
      }

      public float GetMaxHealth()
      {
         return GetComponent<BaseStats>().GetStat(Stat.Health);
      }

      public float GetHealth()
      {
         return health.value;
      }

      public JToken CaptureAsJToken()
      {
         return JToken.FromObject(health.value);
      }

      public void RestoreFromJToken(JToken state)
      {
         health.value = state.ToObject<float>();
         UpdateState();
      }
   }
}
