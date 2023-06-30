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

      private void Start()
      {
         GetComponent<PlayableDirector>().played +=  PlayableDirector_played;
         GetComponent<PlayableDirector>().stopped +=  PlayableDirector_stopped;
      }

      private void PlayableDirector_stopped(PlayableDirector director)
      {
         EnabeControl();
      }

      private void PlayableDirector_played(PlayableDirector director)
      {
         DisableControl();
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
