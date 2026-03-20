using Astras_PSA_Mod.Core.GUIHelpers;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Astras_PSA_Mod.Core;

public class Main : MonoBehaviour
{
    private Rect Wrect = new Rect(250, 250, 220, 220);
    private bool Open = false;
    private bool InitStyles = false;
    private Texture2D? WTex, BTex, STex, STHTex;
    private GUIStyle? WStyle, BStyle, SStyle, STHStyle;
    private Color WindowColor = new Color(0.1f, 0.1f, 0.1f, 1f);
    private Color ButtonColor = new Color(0.2f, 0.2f, 0.2f, 1f);
    private Color sliderTrackColor = new Color(0.15f, 0.15f, 0.15f, 1f);
    private Color sliderThumbColor = new Color(0.0f, 0.6f, 1f, 1f);
    private float Speed = 0f;
    private float MexGroundDis = 0.5f;
    private bool UseJoySticks = true;
    private bool PSAEnabled = false;
    private Vector3 velocity;

    private void OnGUI()
    {
        if (!InitStyles)
        {
            INIT();
            InitStyles = true;
        }
        if (Open)
        {
            Wrect = GUILayout.Window(85674, Wrect, UIM, "Astras PSA Mod", WStyle);
        }
    }

    private void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            Open = !Open;
        }
    }

    private void FixedUpdate()
    {
        if (PSAEnabled)
        {
            PSAMod(Speed, MexGroundDis, UseJoySticks);
        }
    }

    private void UIM(int id)
    {
        PSAWindow();
        GUILayout.Space(10f);
        if (GUILayout.Button("Close", BStyle))
        {
            Open = !Open;
        }
        GUI.DragWindow();
    }

    private void PSAWindow()
    {
        GUILayout.Label("Enable PSA");
        PSAEnabled = GUILayout.Toggle(PSAEnabled, "Enable PSA");
        GUILayout.Label("Set Joy Sticks");
        UseJoySticks = GUILayout.Toggle(UseJoySticks,
        UseJoySticks ? "Right Stick" : "Left Stick");
        GUILayout.Space(5f);
        Speed = GUILayout.HorizontalSlider(Speed, 1f, 12f, SStyle, STHStyle);
        GUILayout.Label($"Speed {Speed:F1}");
        MexGroundDis = GUILayout.HorizontalSlider(MexGroundDis, 0.1f, 2f, SStyle, STHStyle);
        GUILayout.Label($"Ground Dist: {MexGroundDis:F2}");
    }

    private void PSAMod(float speed, float maxGroundDist, bool useRightStick)
    {
        if (GTPlayer.Instance == null || GTPlayer.Instance.bodyCollider == null) return;
        RaycastHit hit;
        float groundY = 0f;
        bool grounded = false;
        if (Physics.Raycast(
            GTPlayer.Instance.transform.position,
            Vector3.down,
            out hit,
            10f,
            GTPlayer.Instance.locomotionEnabledLayers))
        {
            groundY = hit.point.y;
            float playerY = GTPlayer.Instance.bodyCollider.bounds.min.y;
            grounded = (playerY - groundY) <= maxGroundDist;
        }
        Vector2 input = useRightStick
            ? ControllerInputPoller.instance.rightControllerPrimary2DAxis
            : ControllerInputPoller.instance.leftControllerPrimary2DAxis;
        if (input.magnitude < 0.05f)
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, 6f * Time.deltaTime);
            return;
        }
        Transform head = GTPlayer.Instance.headCollider.transform;
        Vector3 forward = head.forward;
        forward.y = 0f;
        forward.Normalize();
        Vector3 right = head.right;
        right.y = 0f;
        right.Normalize();
        Vector3 targetDir = (forward * input.y + right * input.x).normalized;
        float accel = Mathf.Lerp(6f, 14f, speed / 10f);     
        float airControl = Mathf.Lerp(0.3f, 0.7f, speed / 10f); 
        float drag = Mathf.Lerp(0.90f, 0.98f, speed / 10f); 
        float currentAccel = grounded ? accel : accel * airControl;
        velocity = Vector3.Lerp(velocity, targetDir * speed, currentAccel * Time.deltaTime);
        GTPlayer.Instance.transform.position += velocity * Time.deltaTime;
        if (!grounded)
        {
            velocity *= drag;
        }
        float currentY = GTPlayer.Instance.bodyCollider.bounds.min.y;
        if (grounded && currentY < groundY)
        {
            Vector3 pos = GTPlayer.Instance.transform.position;
            pos.y = Mathf.Lerp(pos.y, pos.y + (groundY - currentY), 12f * Time.deltaTime);
            GTPlayer.Instance.transform.position = pos;
        }
    }

    private void INIT()
    {
        WTex = Texturing.MakeTex(1, 1, WindowColor);
        BTex = Texturing.MakeTex(1, 1, ButtonColor);
        STex = Texturing.MakeTex(1, 1, sliderTrackColor);
        STHTex = Texturing.MakeTex(1, 1, sliderThumbColor);
        WStyle = new GUIStyle(GUI.skin.window);
        WStyle.normal.background = WTex;
        WStyle.hover.background = WTex;
        WStyle.active.background = WTex;
        WStyle.focused.background = WTex;
        WStyle.onNormal.background = WTex;
        WStyle.onHover.background = WTex;
        WStyle.onActive.background = WTex;
        WStyle.onFocused.background = WTex;
        WStyle.normal.textColor = Color.white;
        WStyle.fontStyle = FontStyle.Normal;
        BStyle = new GUIStyle(GUI.skin.button);
        BStyle.normal.background = BTex;
        BStyle.active.background = BTex;
        BStyle.hover.background = BTex;
        BStyle.focused.background = BTex;
        BStyle.onNormal.background = BTex;
        BStyle.onActive.background = BTex;
        BStyle.onHover.background = BTex;
        BStyle.onFocused.background = BTex;
        BStyle.normal.textColor = Color.white;
        BStyle.hover.textColor = Color.blue;
        BStyle.active.textColor = Color.red;
        BStyle.focused.textColor = Color.white;
        BStyle.onNormal.textColor = Color.blue;
        BStyle.onHover.textColor = Color.blue;
        BStyle.onActive.textColor = Color.blue;
        BStyle.onFocused.textColor = Color.blue;
        SStyle = new GUIStyle(GUI.skin.horizontalSlider);
        STHStyle = new GUIStyle(GUI.skin.horizontalSliderThumb);
        SStyle.normal.background = STex;
        SStyle.active.background = STex;
        SStyle.hover.background = STex;
        STHStyle.normal.background = STHTex;
        STHStyle.active.background = STHTex;
        STHStyle.hover.background = STHTex;
    }
}