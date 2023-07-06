using System;
using System.Collections;
using RPG.Tags;
using UnityEngine;

namespace RPG.Combat
{
   public class WeaponPickup : MonoBehaviour
   {
      [SerializeField] private Weapon pickupWeapon;
      [SerializeField] private float respawnTime = 3f;

      private void OnTriggerEnter(Collider other)
      {
         if(other.gameObject.tag == Tag.Player.ToString())
         {
            other.GetComponent<Fighter>().EquipWeapon(pickupWeapon);
            StartCoroutine(HideForSeconds(respawnTime));
         }
      }

      private IEnumerator HideForSeconds(float seconds)
      {
         ShowPickup(false);
         yield return new WaitForSeconds(seconds);
         ShowPickup(true);
      }

      private void ShowPickup(bool show)
      {
         GetComponent<CapsuleCollider>().enabled = show;
         foreach(Transform child in transform)
         {
            child.gameObject.SetActive(show);
         }
      }
   }
}