using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
   public class SavingWrapper : MonoBehaviour
   {
      private const string SAVE_FILE = "save";
      private void Update()
      {
         if(Input.GetKeyDown(KeyCode.S))
         {
            GetComponent<SavingSystem>().Save(SAVE_FILE);
         }
         if(Input.GetKeyDown(KeyCode.L))
         {
            GetComponent<SavingSystem>().Load(SAVE_FILE);
         }
      }
   }
}