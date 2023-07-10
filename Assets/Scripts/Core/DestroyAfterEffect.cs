using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterEffect : MonoBehaviour
{
   [SerializeField] GameObject targetToDestroy;

   private void Update()
   {
      if(!GetComponent<ParticleSystem>().IsAlive())
      {
         if(targetToDestroy == null)
         {
            Destroy(gameObject);
         }
         else
         {
            Destroy(targetToDestroy);
         }
      }
   }
}
