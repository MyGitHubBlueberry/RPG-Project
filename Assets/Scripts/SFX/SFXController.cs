using System.Linq;
using UnityEngine;

namespace RPG.SFX
{
   public class SFXController : MonoBehaviour
   {
      [SerializeField] private SFXPlayer[] sfxPlayers;
      private ISFXEvent[] sfxEvents; 

      private void Awake()
      {
         sfxEvents = GetComponents<ISFXEvent>();   
      }

      private void OnEnable()
      {
         if(sfxEvents == null) return;

         foreach(ISFXEvent sfxEvent in sfxEvents)
         {
            sfxEvent.OnSFXTriggerRequest += PlayFromParameter;
         }
      }

      private void OnDisable()
      {
         if(sfxEvents == null) return;

         foreach(ISFXEvent sfxEvent in sfxEvents)
         {
            sfxEvent.OnSFXTriggerRequest -= PlayFromParameter;
         }
      }

      private void PlayFromParameter(SFXParameter parameter, SFXPlayer ownPlayer)
      {
         if(ownPlayer is null)
         {
            SFXPlayer sfxPlayer = sfxPlayers.Where(player => player.GetSFXParameter() == parameter).FirstOrDefault();
            sfxPlayer.PlayClip();
         }
         else
         {
            ownPlayer.PlayClip();
         }
      }
   }
}