using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
   public class SavingWrapper : MonoBehaviour
   {
      private const string SAVE_FILE = "save";

      [SerializeField] float fadeInTime = .3f;

      private Fader fader;
      private void Awake()
      {
         fader = FindObjectOfType<Fader>();
         StartCoroutine(LoadLastScene());
      }

      private IEnumerator LoadLastScene()
      {
         fader.FadeOutImmediate();
         yield return GetComponent<SavingSystem>().LoadLastScene(SAVE_FILE);
         yield return fader.FadeIn(fadeInTime);
      }

      private void Update()
      {
         if(Input.GetKeyDown(KeyCode.S))
         {
            Save();
         }
         if (Input.GetKeyDown(KeyCode.L))
         {
            Load();
         }
         if(Input.GetKeyDown(KeyCode.Delete))
         {
            Delete();
         }
      }

      public void Load()
      {
         GetComponent<SavingSystem>().Load(SAVE_FILE);
      }

      public void Save()
      {
         GetComponent<SavingSystem>().Save(SAVE_FILE);
      }

      public void Delete()
      {
         GetComponent<SavingSystem>().Delete(SAVE_FILE);
      }
   }
}