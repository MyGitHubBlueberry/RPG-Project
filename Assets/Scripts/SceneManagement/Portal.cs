using UnityEngine;
using RPG.Tags;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.AI;
using RPG.SceneManagement.Scenes;
using RPG.Saving;

namespace RPG.SceneManagement
{
   public class Portal : MonoBehaviour
   {
      [SerializeField] private Scenes.Scene targetScene;
      [SerializeField] private Transform spawnPoint;
      [SerializeField] private DestionationIdentifier destination;
      [Min(0)]
      [SerializeField] private float fadeOutTime = .5f;
      [Min(0)]
      [SerializeField] private float fadeWaitTime = .5f;
      [Min(0)]
      [SerializeField] private float fadeInTime = 1f;

      private enum DestionationIdentifier
      {
         None, A, B, C,
      }


      private void OnTriggerEnter(Collider other)
      {
         if(other.gameObject.tag != Tag.Player.ToString()) return;
         
         StartCoroutine(Transition());
      }

      private IEnumerator Transition()
      {
         if(targetScene == Scenes.Scene.None)
         {
            Debug.LogError("<color=orange>Scene to load</color> not set");
            yield break;
         }

         DontDestroyOnLoad(gameObject);

         Fader fader = FindObjectOfType<Fader>();
         SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

         yield return fader.FadeOut(fadeOutTime);

         savingWrapper.Save();

         yield return SceneManager.LoadSceneAsync(targetScene.ToString());

         savingWrapper.Load();

         Portal destination = GetOtherPortal();
         UpdatePlayer(destination);

         savingWrapper.Save();

         yield return new WaitForSeconds(fadeWaitTime);
         yield return fader.FadeIn(fadeInTime);

         Destroy(gameObject);
      }

      private void UpdatePlayer(Portal destination)
      {
         GameObject player = GameObject.FindGameObjectWithTag(Tag.Player.ToString());
         player.GetComponent<NavMeshAgent>().Warp(destination.spawnPoint.position);
         player.transform.rotation = destination.spawnPoint.rotation;
      }

      private Portal GetOtherPortal()
      {
         if(destination == DestionationIdentifier.None) 
         {
            Debug.LogError("<color=orange>Scene to load</color> not set");
            return null;
         }

         foreach(Portal portal in FindObjectsOfType<Portal>())
         {
            if(portal.destination != destination || portal == this) continue;

            return portal;
         }
         return null;
      }
   }
}