using UnityEngine;

namespace Astras_PSA_Mod.Core.GUIHelpers;

public static class Texturing
{
    public static Texture2D MakeTex(int HH, int WW, Color col)
    {
        Texture2D Vall = new Texture2D(HH, WW);
        Vall.SetPixel(0, 0, col);
        Vall.Apply();
        return Vall;
    }
}