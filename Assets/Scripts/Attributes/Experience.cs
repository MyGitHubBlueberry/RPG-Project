using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;

namespace RPG.Attributes
{   
   public class Experience : MonoBehaviour, ISaveable
   {
      [SerializeField] private float experience = 0f;

      

      public void GainExperience(float experience)
      {
         this.experience += Mathf.Max(experience, 0f);
      }
      public JToken CaptureAsJToken()
      {
         return JToken.FromObject(experience);
      }
      public void RestoreFromJToken(JToken state)
      {
         experience = state.ToObject<float>();
      }
   }
}
