using UnityEngine;

namespace RPG.Attributes
{
   public class HealthBar : MonoBehaviour
   {
      [SerializeField] private Health health;
      [SerializeField] private RectTransform foreground;
      
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
         foreground.localScale = new Vector3(health.GetPercentage() / 100, 1 ,1);
      }
   }
}