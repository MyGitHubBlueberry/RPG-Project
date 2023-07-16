using RPG.Attributes;
using UnityEngine;

namespace RPG.UI
{
   public class DamageTextSpawner : MonoBehaviour
   {
      [SerializeField] private DamageText damageTextPrefab;

      private Health health;

      private void Awake()
      {
         health = GetComponentInParent<Health>();
      }

      private void OnEnable()
      {
         health.OnTakeDamage += Spawn;
      }

      private void OnDisable()
      {
         health.OnTakeDamage -= Spawn;
      }

      private void Spawn(float damageAmmount)
      {
         DamageText instance = Instantiate<DamageText>(damageTextPrefab, transform);
         instance.SetValue(damageAmmount);
      }
   }
}
