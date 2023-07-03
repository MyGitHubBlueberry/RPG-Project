using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
   [ExecuteAlways]
   public class SaveableEntity : MonoBehaviour
   {
      [SerializeField] string uniqueIdentifier = "";
      // CACHED STATE
      static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();
      public string GetUniqueIdentifier()
      {
         return uniqueIdentifier;
      }
 
      public JToken CaptureAsJtoken()
      {
         JObject state = new JObject();
         IDictionary<string, JToken> stateDict = state;
         foreach (ISaveable saveable in GetComponents<ISaveable>())
         {
            JToken token = saveable.CaptureAsJToken();
            string component = saveable.GetType().ToString();
            Debug.Log($"{name} Capture {component} = {token.ToString()}");
            stateDict[saveable.GetType().ToString()] = token;
         }
         return state;
     }

     public void RestoreFromJToken(JToken s) 
     {
         JObject state = s.ToObject<JObject>();
         IDictionary<string, JToken> stateDict = state;
         foreach (ISaveable saveable in GetComponents<ISaveable>())
         {
            string component = saveable.GetType().ToString();
            if (stateDict.ContainsKey(component))
            {
               Debug.Log($"{name} Restore {component} =>{stateDict[component].ToString()}");
               saveable.RestoreFromJToken(stateDict[component]);
            }
         }
     }

#if UNITY_EDITOR
     private void Update() {
         if (Application.IsPlaying(gameObject)) return;
         if (string.IsNullOrEmpty(gameObject.scene.path)) return;
         SerializedObject serializedObject = new SerializedObject(this);
         SerializedProperty property = serializedObject.FindProperty(nameof(uniqueIdentifier));
         
         if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
         {
            property.stringValue = System.Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
         }
         globalLookup[property.stringValue] = this;
     }
#endif

      private bool IsUnique(string candidate)
      {
         if (!globalLookup.ContainsKey(candidate)) return true;
         if (globalLookup[candidate] == this) return true;
         if (globalLookup[candidate] == null)
         {
            globalLookup.Remove(candidate);
            return true;
         }
         if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
         {
            globalLookup.Remove(candidate);
            return true;
         }
         return false;
      }
   }
}
