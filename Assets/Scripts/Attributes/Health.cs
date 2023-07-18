using System;
using UnityEngine;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using GameDevTV.Utils;
using RPG.Animation;
using RPG.SFX;

namespace RPG.Attributes
{
   public class Health : MonoBehaviour, ISaveable, IAnimationTriggerEvent, ISFXEvent
   {
      [SerializeField] private float regenerationPercentage = 70;

      public event EventHandler<IAnimationTriggerEvent.OnResetSetAnimationTriggerRequestEventArgs> OnResetSetAnimationTriggerRequest;
      public event Action<SFXParameter,SFXPlayer> OnSFXTriggerRequest;
      public event Action<float> OnTakeDamage;
      public event Action OnHealthRegenerated;
      public event Action OnZeroHealth;

      private LazyValue<float> health;
      private bool isDead;
      
      private void Awake()
      {
         health = new LazyValue<float>(GetInitialHealth);
      }
      private void OnEnable()
      {
         GetComponent<BaseStats>().OnLevelUp += RegenerateHealthPercent;
      }

      private void Start() => health.ForceInit();

      private void OnDisable()
      {
         GetComponent<BaseStats>().OnLevelUp -= RegenerateHealthPercent;
      }
      
      private float GetInitialHealth()
      {
         return GetComponent<BaseStats>().GetStat(Stat.Health);
      }

      private void RegenerateHealthPercent()
      {
         float healthAfterRegen = GetMaxHealth() * regenerationPercentage / 100;
         float healthToRegenerate = (GetPercentage() > regenerationPercentage) ? 0 : healthAfterRegen - health.value;
         RegenerateHealth(healthToRegenerate);
      }

      private void Die(bool isRestoredState = false)
      {
         if(isDead) return;

         isDead = true;
         GetComponent<ActionScheduler>().CancelCurrentAction();
         OnZeroHealth?.Invoke();
         
         OnResetSetAnimationTriggerRequest?.Invoke(this,new IAnimationTriggerEvent.OnResetSetAnimationTriggerRequestEventArgs
         {
            resetTrigger = AnimatorParameters.Trigger.die,
            setTrigger = AnimatorParameters.Trigger.die,
         });

         if(!isRestoredState) OnSFXTriggerRequest?.Invoke(SFXParameter.Death, null);
      }

      private void UpdateState()
      {
         if(health.value <= 0)
         {
            Die(true);
         }
      }

      private void AwardExperience(GameObject instigator)
      {
         Experience experience = instigator.GetComponent<Experience>();
         
         if(experience == null) return;

         float rewardXP = GetComponent<BaseStats>().GetStat(Stat.ExperienceReward);
         experience.GainExperience(rewardXP);
      }

      public void RegenerateHealth(float regenHealthPoints)
      {
         if(Mathf.Approximately(health.value, GetMaxHealth())) return;
         health.value = Mathf.Min(GetMaxHealth(), health.value + regenHealthPoints);

         OnHealthRegenerated?.Invoke();
         OnSFXTriggerRequest?.Invoke(SFXParameter.HealSpell, null);
      }

      public void TakeDamage(GameObject instigator,float damage)
      {
         health.value = Mathf.Max(health.value - damage, 0f);
         
         OnTakeDamage?.Invoke(damage);
         OnSFXTriggerRequest?.Invoke(SFXParameter.TakeDamage, null);


         if(health.value == 0)
         {
            Die();
            AwardExperience(instigator);
         }
      }

      public bool IsDead()
      {
         return isDead;
      }

      public bool IsAlive()
      {
         return !isDead;
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
