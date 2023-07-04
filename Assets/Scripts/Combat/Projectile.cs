using System;
using UnityEngine;

namespace RPG.Combat
{
   public class Projectile : MonoBehaviour
   {
      [SerializeField] private Transform target;
      [SerializeField] private float speed;

      private void Update()
      {
         if(target == null) return;
         

         transform.LookAt(GetAimLocation());
         transform.Translate(Vector3.forward * speed * Time.deltaTime);
      }

      private Vector3 GetAimLocation()
      {
         CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
      

         return (targetCollider == null) ? 
            (target.position) : 
            (target.position + Vector3.up * targetCollider.height / 2);
      }
   }
}
