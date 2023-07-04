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
      }

      private IEnumerator Start()
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
      }

      public void Load()
      {
         GetComponent<SavingSystem>().Load(SAVE_FILE);
      }

      public void Save()
      {
         GetComponent<SavingSystem>().Save(SAVE_FILE);
      }
   }
}