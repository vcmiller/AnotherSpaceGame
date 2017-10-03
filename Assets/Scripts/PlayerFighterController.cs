using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFighterController : PlayerController<FighterProxy> {
    public float mouseToRotationScale = 0.01f;
    public CrosshairMotor crosshair;

    #region axes
    public void Axis_Horizontal(float value) {
        controlled.rotation += transform.forward * -value;
    }

    public void Axis_Vertical(float value) {
        controlled.thrust = value;
    }

    public void Axis_LookHorizontal(float value) {
        controlled.rotation += transform.up * value;

        if (Mathf.Abs(value) != 0) {
            controlled.crosshair = Vector2.zero;
        }
    }

    public void Axis_LookVertical(float value) {
        controlled.rotation -= transform.right * value;

        if (Mathf.Abs(value) != 0) {
            controlled.crosshair = Vector2.zero;
        }
    }

    public void Axis_MouseX(float value) {
        controlled.crosshair += Vector2.right * value;
    }

    public void Axis_MouseY(float value) {
        controlled.crosshair += Vector2.up * value;
    }
    #endregion
    #region buttons
    // Technically an axis but used as a button. Needs to be axis for analog triggers.
    public void Axis_Fire(float value) {
        if (value > 0.5f) {
            controlled.firing = true;
        }
    }
    #endregion

    public override void GetInput() {
        base.GetInput();

        Vector2 c = controlled.crosshair;

        controlled.rotation += (transform.right * -c.y + transform.up * c.x) * mouseToRotationScale;

        controlled.target = viewTarget.transform.position + (GetComponentInChildren<TargetTrackerMotor>().transform.position - viewTarget.transform.position).normalized * 1000;
    }

    protected override void OnEnable() {
        base.OnEnable();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    protected override void OnDisable() {
        base.OnDisable();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
