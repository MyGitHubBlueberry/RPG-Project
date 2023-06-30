using UnityEngine;
using RPG.Tags;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene(targetScene.ToString());
         }
      }
   }
}