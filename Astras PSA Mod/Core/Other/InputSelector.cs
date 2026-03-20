using Astras_PSA_Mod.Core.Libs;
using UnityEngine;

namespace Astras_PSA_Mod.Core.Other;

public static class InputSelector
{
    public static string[] InputNames =
    {
        "Right Joystick",
        "Left Joystick"
    };

    public static Func<Vector2>[] AxisInputs =
    {
        () => InputLib.RightJoystick,
        () => InputLib.LeftJoystick
    };

    public static int SelectedIndex = 0;

    public static Vector2 Axis => AxisInputs[SelectedIndex]?.Invoke() ?? Vector2.zero;
}

