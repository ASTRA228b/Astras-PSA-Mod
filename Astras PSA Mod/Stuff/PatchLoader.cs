using HarmonyLib;

namespace Astras_PSA_Mod.Stuff;

public class PatchLoader
{
    public static void Apply()
    {
        Harmony VALLL = new Harmony(Constantss.GUID);
        VALLL.PatchAll();
    }
}