using UnityEngine;

namespace RPG.Combat
{   
   public class Weapon : MonoBehaviour
   {
      public void OnHit()
      {
         print("Weapon hit " +  gameObject.name);
      }
   }
}