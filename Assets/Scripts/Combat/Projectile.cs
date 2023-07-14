using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
   public class Projectile : MonoBehaviour
   {
      [SerializeField] private float speed;
      [SerializeField] private bool isHoming;
      [SerializeField] private GameObject hitEffect;
      [SerializeField] private float maxLifeTime = 10f;
      [SerializeField] private GameObject[] destroyOnHit;
      [SerializeField] private float lifeAfterImpact = .5f;


      private Health target;
      private GameObject instigator;
      private float damage = 0f;

      private void Start()
      {
         transform.LookAt(GetAimLocation());
      }

      private void Update()
      {
         if(target == null) return;

         if(isHoming && target.IsAlive()) transform.LookAt(GetAimLocation());

         transform.Translate(Vector3.forward * speed * Time.deltaTime);
      } 

      private void OnTriggerEnter(Collider other)
      {
         if(other.GetComponent<Health>() != target) return;
         if(target.IsDead()) return;

         target.TakeDamage(instigator, damage);

         speed = 0f;

         if(hitEffect != null)
         {
            GameObject instantiatedEffect = Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            ParticleSystem particles = instantiatedEffect.GetComponentInChildren<ParticleSystem>();
            float totalDuration = particles.main.duration + particles.main.startLifetime.constantMax;
            Destroy(instantiatedEffect, totalDuration);
         }

         foreach(GameObject toDestroy in destroyOnHit)
         {
            Destroy(toDestroy);
         }

         Destroy(gameObject, lifeAfterImpact);
      }

      private Vector3 GetAimLocation()
      {
         CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
      

         return (targetCollider == null) ? 
            (target.transform.position) : 
            (target.transform.position + Vector3.up * targetCollider.height / 2);
      }

      public void SetTarget(Health target, GameObject instigator, float damage)
      {
         this.target = target;
         this.damage = damage;
         this.instigator = instigator;

         Destroy(gameObject, maxLifeTime);
      }
   }
}
