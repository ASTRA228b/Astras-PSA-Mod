
using Valve.VR;

namespace Astras_PSA_Mod.Core.Libs;

public static class InputLib
{
    public static UnityEngine.Vector2 RightJoystick
    {
        get
        {
            if (SteamVR_Actions.gorillaTag_RightJoystick2DAxis == null)
                return UnityEngine.Vector2.zero;

            var axis = SteamVR_Actions.gorillaTag_RightJoystick2DAxis.axis;
            return new UnityEngine.Vector2(axis.x, axis.y);
        }
    }

    public static UnityEngine.Vector2 LeftJoystick
    {
        get
        {
            if (SteamVR_Actions.gorillaTag_LeftJoystick2DAxis == null)
                return UnityEngine.Vector2.zero;

            var axis = SteamVR_Actions.gorillaTag_LeftJoystick2DAxis.axis;
            return new UnityEngine.Vector2(axis.x, axis.y);
        }
    }
}