using UnityEngine;

namespace RPG.SFX
{   
   [CreateAssetMenu(fileName = "SFX", menuName = "SFX/New SFX")]
   public class SFX : ScriptableObject
   {  
      [SerializeField] private SFXParameter sfxParameter;
      [SerializeField] private AudioClip[] audioClips;

      internal SFXParameter GetSFXParameter()
      {
         return sfxParameter;
      }

      internal AudioClip GetRandomClip()
      {
         if(audioClips == null) return null;
         return audioClips[Random.Range(0, audioClips.Length)];
      }
   }
}
