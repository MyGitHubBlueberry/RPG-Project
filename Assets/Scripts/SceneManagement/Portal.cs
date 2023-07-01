using UnityEngine;
using RPG.Tags;
using UnityEngine.SceneManagement;
using System.Collections;

namespace RPG.SceneManagement
{
   public class Portal : MonoBehaviour
   {
      [SerializeField] private Scene targetScene;

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
         
         print("Scene Loaded");
         Destroy(gameObject);
      }

   }
}