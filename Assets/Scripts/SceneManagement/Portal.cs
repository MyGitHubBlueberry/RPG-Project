using UnityEngine;
using RPG.Tags;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
   public class Portal : MonoBehaviour
   {
      [SerializeField] private Scene targetScene;
      [SerializeField] private Transform spawnPoint;

      private enum Scene
      {
         None,
         SandboxScene,
         Sandbox2Scene,
      }

      private void OnTriggerEnter(Collider other)
      {
         if(other.gameObject.tag != Tag.Player.ToString()) return;

         if(targetScene != Scene.None)
         {
            StartCoroutine(Transition());
         }
      }

      private IEnumerator Transition()
      {
         DontDestroyOnLoad(gameObject);
         yield return SceneManager.LoadSceneAsync(targetScene.ToString());

         Portal linkedPortal = GetLinkedPortal();
         UpdatePlayer(linkedPortal);

         Destroy(gameObject);
      }

      private void UpdatePlayer(Portal linkedPortal)
      {
         GameObject player = GameObject.FindGameObjectWithTag(Tag.Player.ToString());
         player.GetComponent<NavMeshAgent>().Warp(linkedPortal.spawnPoint.position);
         player.transform.rotation = linkedPortal.spawnPoint.rotation;
      }

      private Portal GetLinkedPortal()
      {
         foreach(Portal portal in FindObjectsOfType<Portal>())
         {
            if(portal == this) continue;

            return portal;
         }
         return null;
      }
   }
}