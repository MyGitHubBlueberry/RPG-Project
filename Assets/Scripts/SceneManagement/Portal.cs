using UnityEngine;
using RPG.Tags;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
   public class Portal : MonoBehaviour
   {
      [SerializeField] private Scene targetScene;
      [SerializeField] private Transform spawnPoint;
      [SerializeField] private DestionationIdentifier destination;
      [Min(0)]
      [SerializeField] private float fadeOutTime = .5f;
      [Min(0)]
      [SerializeField] private float fadeWaitTime = .5f;
      [Min(0)]
      [SerializeField] private float fadeInTime = 1f;

      //order in enum is improtant
      private enum Scene
      {
         None = -1,
         SandboxScene,
         Sandbox2Scene,
      }

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
         if(targetScene == Scene.None)
         {
            Debug.LogError("<color=orange>Scene to load</color> not set");
            yield break;
         }

         DontDestroyOnLoad(gameObject);

         Fader fader = FindObjectOfType<Fader>();
         SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

         yield return fader.FadeOut(fadeOutTime);
         
         savingWrapper.Save();
         

         yield return SceneManager.LoadSceneAsync((int)targetScene);

         savingWrapper.Load();

         Portal destination = GetOtherPortal();
         UpdatePlayer(destination);

         //Throws error if save there

         yield return new WaitForSeconds(fadeWaitTime);
         yield return fader.FadeIn(fadeInTime);

         savingWrapper.Save();

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