using UnityEngine;

namespace RPG.Attributes
{
   public class HealthBar : MonoBehaviour
   {
      [SerializeField] private Health health;
      [SerializeField] private RectTransform foreground;
      [SerializeField] private Canvas canvas;
      
      private void OnEnable()
      {
         health.OnHealthRegenerated += UpdateDisplay; 
         health.OnTakeDamage += _ => UpdateDisplay();
      }

      private void Start() => UpdateDisplay();

      private void OnDisable()
      {
         health.OnHealthRegenerated -= UpdateDisplay; 
         health.OnTakeDamage -= _ => UpdateDisplay();
      }

      private void UpdateDisplay()
      {
         float healthPersentage = health.GetPercentage() / 100;
         foreground.localScale = new Vector3(healthPersentage, 1 ,1);
         canvas.enabled = !(Mathf.Approximately(healthPersentage,0) || Mathf.Approximately(healthPersentage,1));
      }
   }
}