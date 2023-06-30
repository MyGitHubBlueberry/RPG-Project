using RPG.Core;
using RPG.Tags;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
   public class CinematicTrigger : MonoBehaviour
   {
      bool alreadyTriggered;
      private void OnTriggerEnter(Collider other)
      {
         if(other.gameObject.tag != Tag.Player.ToString() || alreadyTriggered) return;

         alreadyTriggered = true;
         GetComponent<PlayableDirector>().Play();
      }
   }
}
