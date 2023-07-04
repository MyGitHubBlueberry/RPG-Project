using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
   public class Projectile : MonoBehaviour
   {
      [SerializeField] private float speed;

      private Health target;

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
            (target.transform.position) : 
            (target.transform.position + Vector3.up * targetCollider.height / 2);
      }

      public void SetTarget(Health target)
      {
         this.target = target;
      }
   }
}
