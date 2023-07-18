using UnityEngine;

namespace RPG.SFX
{
   [RequireComponent(typeof(AudioSource))]
   public class SFXPlayer : MonoBehaviour
   {
      [SerializeField] private SFX sfx;
      private AudioSource audioSource;


      private void Awake()
      {
         audioSource = GetComponent<AudioSource>();
      }

      internal SFXParameter GetSFXParameter()
      {
         return sfx.GetSFXParameter();
      }

      internal void PlayClip()
      {
         audioSource.PlayOneShot(sfx.GetRandomClip());
         print("playing");
      }
   }
}