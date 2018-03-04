using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public class PlayerFighterController : PlayerController {
    public float mouseToRotationScale = 0.01f;
    public Vector2 crosshairLimits = new Vector2(300, 150);

    public CrosshairMotor crosshair { get; private set; }
    public TargetTrackerMotor tracker { get; private set; }
    public FighterChannels controlled { get; private set; }

    private bool lockPressed;
    private bool mouseAim;

    #region axes
    public void Axis_Horizontal(float value) {
        controlled.rotation += transform.forward * -value;
    }

    public void Axis_Vertical(float value) {
        controlled.thrust = value;
    }

    public void Axis_LookHorizontal(float value) {
        if (value != 0) {
            controlled.rotation += transform.up * value;
            controlled.crosshair = Vector2.zero;
            mouseAim = false;
        }
    }

    public void Axis_LookVertical(float value) {
        if (value != 0) {
            print(value);
            controlled.rotation -= transform.right * value;
            controlled.crosshair = Vector2.zero;
            mouseAim = false;
        }
    }

    public void Axis_MouseX(float value) {
        if (value != 0) {
            Vector3 v = controlled.crosshair + Vector3.right * value;
            v.x = Mathf.Clamp(v.x, -crosshairLimits.x, crosshairLimits.x);
            controlled.crosshair = v;
            mouseAim = true;
        }
    }

    public void Axis_MouseY(float value) {
        if (value != 0) {
            Vector3 v = controlled.crosshair + Vector3.up * value;
            v.y = Mathf.Clamp(v.y, -crosshairLimits.y, crosshairLimits.y);
            controlled.crosshair = v;
            mouseAim = true;
        }
    }
    #endregion
    #region buttons
    // Technically an axis but used as a button. Needs to be axis for analog triggers.
    public void Axis_Fire(float value) {
        if (value > 0.5f) {
            controlled.firing = true;
        }
    }

    public void Axis_Lock(float value) {
        if (value > 0.5f && !lockPressed) {
            float minDist = float.PositiveInfinity;
            TargetableObject targ = null;

            foreach (var f in FindObjectsOfType<Fighter>()) {
                if (f.transform != transform) {
                    Vector3 vpos = viewTarget.camera.WorldToScreenPoint(f.transform.position);
                    if (vpos.z > 0) {
                        float dist = vpos.sqrMagnitude;
                        if (dist < minDist) {
                            minDist = dist;
                            targ = f;
                        }
                    }
                }
            }

            if (targ != null) {
                controlled.target = targ;
            }

            lockPressed = true;
        } else if (value < 0.5f) {
            lockPressed = false;
        }
    }
    #endregion

    public override void GetInput() {
        base.GetInput();

        Vector2 c = controlled.crosshair;

        controlled.rotation += (transform.right * -c.y + transform.up * c.x) * mouseToRotationScale;

        if (mouseAim || !tracker.image.enabled) {
            controlled.aim = viewTarget.transform.position + (crosshair.transform.position - viewTarget.transform.position).normalized * 1000;
        } else {
            controlled.aim = viewTarget.transform.position + (tracker.transform.position - viewTarget.transform.position).normalized * 1000;
        }
    }

    protected override void OnEnable() {
        base.OnEnable();

        controlled = channels as FighterChannels;
        crosshair = GetComponentInChildren<CrosshairMotor>();
        tracker = GetComponentInChildren<TargetTrackerMotor>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    protected override void OnDisable() {
        base.OnDisable();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
