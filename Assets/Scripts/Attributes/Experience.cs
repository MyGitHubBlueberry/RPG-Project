using UnityEngine;

namespace RPG.Attributes
{   
   public class Experience : MonoBehaviour
   {
      [SerializeField] private float experience = 0f;

      public void GainExperience(float experience)
      {
         this.experience += Mathf.Max(experience, 0f);
      }
   }
}
