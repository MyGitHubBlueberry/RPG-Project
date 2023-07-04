using RPG.Tags;
using UnityEngine;

namespace RPG.Combat
{
   public class WeaponPickup : MonoBehaviour
   {
      [SerializeField] private Weapon pickupWeapon;

      private void OnTriggerEnter(Collider other)
      {
         if(other.gameObject.tag == Tag.Player.ToString())
         {
            other.GetComponent<Fighter>().EquipWeapon(pickupWeapon);
            Destroy(gameObject);
         }
      }
   }
}