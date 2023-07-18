using RPG.SFX;
using UnityEngine;

namespace RPG.Combat
{   
   public class Weapon : MonoBehaviour
   {
      [SerializeField] private SFXParameter attackParameter;
      [SerializeField] private SFXPlayer sfxPlayer;

      public SFXParameter GetSFXParameter()
      {
         return attackParameter;
      }

      public SFXPlayer GetSFXPlayer()
      {
         return sfxPlayer;
      }
   }
}