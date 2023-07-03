using Newtonsoft.Json.Linq;

namespace RPG.Saving
{
   public interface ISaveable
   {
      public JToken CaptureAsJToken();
      public void RestoreFromJToken(JToken state);
   }
}
