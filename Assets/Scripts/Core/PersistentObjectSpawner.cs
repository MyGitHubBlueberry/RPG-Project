using System;
using UnityEngine;

namespace RPG.Core
{
   public class PersistentObjectSpawner : MonoBehaviour
   {
      [SerializeField] private GameObject persistentObjectPrefab;

      private static bool hasSpawned;

      private void Start()
      {
         if(hasSpawned) return;
         
         SpawnPersistentObjects();

         hasSpawned = true;
      }

      private void SpawnPersistentObjects()
      {
         GameObject persistentObject = Instantiate(persistentObjectPrefab);
         DontDestroyOnLoad(persistentObject);
      }
   }
}