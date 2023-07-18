using System;
using System.Collections;
using RPG.Control;
using RPG.Tags;
using UnityEngine;

namespace RPG.Combat
{
   public class WeaponPickup : MonoBehaviour, IRaycastable
   {
      [SerializeField] private WeaponConfig pickupWeapon;
      [SerializeField] private float respawnTime = 3f;

      private void OnTriggerEnter(Collider other)
      {
         if(other.CompareTag(Tag.Player.ToString()))
         {
            Pickup(other.GetComponent<Fighter>());
         }
      }

      private void Pickup(Fighter fighter)
      {
         fighter.EquipWeapon(pickupWeapon);
         StartCoroutine(HideForSeconds(respawnTime));
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

      public bool HandleRaycast(PlayerController callingController)
      {
         if(Input.GetMouseButton(0))
         {
            Pickup(callingController.GetComponent<Fighter>());
         }
         return true;
      }

      public CursorType GetCursorType()
      {
         return CursorType.Pickup;
      }
   }
}