using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
   public class Fader : MonoBehaviour
   {
      private CanvasGroup canvasGroup;
      private Coroutine currentlyActiveFade;

      private void Awake()
      {
         canvasGroup = GetComponent<CanvasGroup>();
      }

      private IEnumerator FadeRoutine(float time, float targetAlpha)
      {
         while(!Mathf.Approximately(canvasGroup.alpha, targetAlpha)) 
         {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, Time.deltaTime / time);
            yield return null;
         } 
      }
      
      public void FadeOutImmediate()
      {
         canvasGroup.alpha = 1f;
      }

      public IEnumerator FadeOut(float time)
      {
         return Fade(time, 1);
      }

      public IEnumerator FadeIn(float time)
      {
         return Fade(time, 0);
      }

      public IEnumerator Fade(float time, float targetAlpha)
      {
         if(currentlyActiveFade != null) StopCoroutine(currentlyActiveFade);
         currentlyActiveFade = StartCoroutine(FadeRoutine(time, targetAlpha));
         yield return currentlyActiveFade;
      }
   }
}
