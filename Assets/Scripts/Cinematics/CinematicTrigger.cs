using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
   public class CinematicTrigger : MonoBehaviour
   {
      bool alreadyTriggered;
      private void OnTriggerEnter(Collider other)
      {
         if(other.gameObject.tag != Tags.Player.ToString() || alreadyTriggered) return;

         alreadyTriggered = true;
         GetComponent<PlayableDirector>().Play();
      }
   }
}
