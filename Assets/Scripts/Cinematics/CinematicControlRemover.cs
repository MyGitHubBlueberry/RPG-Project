using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Tags;
using RPG.Control;

namespace RPG.Cinematics
{
   public class CinematicControlRemover : MonoBehaviour
   {
      private GameObject player;

      private void Awake()
      {
         player = GameObject.FindWithTag(Tag.Player.ToString());         
      }

      private void OnEnable()
      {
         GetComponent<PlayableDirector>().played += _ => DisableControl();
         GetComponent<PlayableDirector>().stopped += _ => EnabeControl();
      }

      private void OnDisable()
      {
         GetComponent<PlayableDirector>().played -= _ => DisableControl();
         GetComponent<PlayableDirector>().stopped -= _ => EnabeControl();
      }

      private void DisableControl()
      {
         player.GetComponent<ActionScheduler>().CancelCurrentAction();
         player.GetComponent<PlayerController>().enabled = false;
      }
      
      private void EnabeControl()
      {
         player.GetComponent<PlayerController>().enabled = true;
      }
   }
}
