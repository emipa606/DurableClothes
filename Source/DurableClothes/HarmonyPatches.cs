using System.Reflection;
using HarmonyLib;
using Verse;

namespace DurableClothes
{
    [StaticConstructorOnStartup]
    public class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("rimworld.mlie.durableclothes");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}