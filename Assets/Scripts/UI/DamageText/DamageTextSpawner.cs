using UnityEngine;

namespace RPG.UI
{
   public class DamageTextSpawner : MonoBehaviour
   {
      [SerializeField] private DamageText damageTextPrefab;

      private void Start()
      {
         Spawn(1);
      }

      private void Spawn(float damageAmmount)
      {
         DamageText instance = Instantiate<DamageText>(damageTextPrefab, transform, true);
      }
   }
}
