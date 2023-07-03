using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
   public class SavingSystem : MonoBehaviour
   {
      private const string FILE_EXTENTION = ".json";
      private const string LAST_SCENE_KEY = "LastScene";

      public IEnumerator LoadLastScene(string saveFile)
      {
         JObject state = LoadJsonFromFile(saveFile);
         IDictionary<string, JToken> stateDict = state; 
         int buildIndex = SceneManager.GetActiveScene().buildIndex;
         if (stateDict.ContainsKey(LAST_SCENE_KEY))
         {
            buildIndex = (int)stateDict[LAST_SCENE_KEY];
         }
         yield return SceneManager.LoadSceneAsync(buildIndex);
         RestoreFromToken(state);
      }

      public void Save(string saveFile)
      {
         JObject state = LoadJsonFromFile(saveFile);
         CaptureAsToken(state);
         SaveFileAsJSon(saveFile,state);
      }

      public void Load(string saveFile)
      {
         RestoreFromToken(LoadJsonFromFile(saveFile));
      }

      public void Delete(string saveFile)
      {
         File.Delete(GetPathFromSaveFile(saveFile));
      }

      public IEnumerable<string> ListSaves()
      {
         foreach (string path in Directory.EnumerateFiles(Application.persistentDataPath))
         {
            if (Path.GetExtension(path) == FILE_EXTENTION)
            {
               yield return Path.GetFileNameWithoutExtension(path);
            }
         }
      }

      private JObject LoadJsonFromFile(string saveFile)
      {
         string path = GetPathFromSaveFile(saveFile);
         print("Saving to " + path);

         if (!File.Exists(path))
         {
            return new JObject();
         }

         using(TextReader textReader = File.OpenText(path))
         using(JsonTextReader jsonTextReader = new JsonTextReader(textReader))
         {
            jsonTextReader.FloatParseHandling = FloatParseHandling.Double;

            return JObject.Load(jsonTextReader);
         }
      }

      private void SaveFileAsJSon(string saveFile, JObject state)
      {
         string path = GetPathFromSaveFile(saveFile);
         print("Saving to " + path);

         using(TextWriter textWriter = File.CreateText(path))
         using(JsonTextWriter jsonTextWriter = new JsonTextWriter(textWriter))
         {
            jsonTextWriter.Formatting = Formatting.Indented;

            state.WriteTo(jsonTextWriter);
         }
      }

      private void CaptureAsToken(JObject state)
      {
         IDictionary<string, JToken> stateDict = state;
         foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
         {
            stateDict[saveable.GetUniqueIdentifier()] = saveable.CaptureAsJtoken();
         }

         stateDict[LAST_SCENE_KEY] = SceneManager.GetActiveScene().buildIndex;
      }

      private void RestoreFromToken(JObject state)
      {
         IDictionary<string, JToken> stateDict = state;
         foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
         {
            string id = saveable.GetUniqueIdentifier();
            if (stateDict.ContainsKey(id))
            {
               saveable.RestoreFromJToken(stateDict[id]);
            }
         }
      }

      private string GetPathFromSaveFile(string saveFile)
      {
         return Path.Combine(Application.persistentDataPath, saveFile + FILE_EXTENTION);
      }
   }
}
