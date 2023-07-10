using System.Collections.Generic;

namespace RPG.Stats
{
   public interface IModifierProvider
   {
      IEnumerable<float> GetAdditiveModifiers(Stat stat);
      IEnumerable<float> GetPersantageModifiers(Stat stat);
      public IEnumerable<float> GetModifiers(Stat stat, IModifierProvider.Modifier modifier)
      {
         switch(modifier)
         {
            case IModifierProvider.Modifier.Additive:
               return GetAdditiveModifiers(stat);
            case IModifierProvider.Modifier.Persantage:
               return GetPersantageModifiers(stat);
         }
         return null;
      }

      enum Modifier
      {
         Additive,
         Persantage,
      }
   }
}
