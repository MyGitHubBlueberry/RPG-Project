using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
   public class Projectile : MonoBehaviour
   {
      [SerializeField] private float speed;
      [SerializeField] private bool isHoming;

      private Health target;
      private float damage = 0f;

      private void Start()
      {
         transform.LookAt(GetAimLocation());
      }

      private void Update()
      {
         if(target == null) return;

         if(isHoming && !target.GetIsDead()) transform.LookAt(GetAimLocation());

         transform.Translate(Vector3.forward * speed * Time.deltaTime);
      } 

      private void OnTriggerEnter(Collider other)
      {
         if(other.GetComponent<Health>() != target) return;
         if(target.GetIsDead()) return;

         target.TakeDamage(damage);
         Destroy(gameObject);
      }

      private Vector3 GetAimLocation()
      {
         CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
      

         return (targetCollider == null) ? 
            (target.transform.position) : 
            (target.transform.position + Vector3.up * targetCollider.height / 2);
      }

      public void SetTarget(Health target, float damage)
      {
         this.target = target;
         this.damage = damage;
      }
   }
}
