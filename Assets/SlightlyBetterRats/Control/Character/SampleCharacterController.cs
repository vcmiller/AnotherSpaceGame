using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCharacterController : PlayerController<CharacterProxy> {
	
	public void Axis_Horizontal(float value) {
        Vector3 right = viewTarget.transform.right;
        right.y = 0;
        right = right.normalized;

        controlled.movement += right * value;
    }

    public void Axis_Vertical(float value) {
        Vector3 fwd = viewTarget.transform.forward;
        fwd.y = 0;
        fwd = fwd.normalized;

        controlled.movement += fwd * value;
    }

    public void ButtonDown_Jump() {
        controlled.jump = true;
    }

    public void Axis_MouseX(float value) {
        controlled.rotation = Quaternion.Euler(0, value, 0) * controlled.rotation;
    }

    public void Axis_MouseY(float value) {
        controlled.rotation *= Quaternion.Euler(-value, 0, 0);
    }
}
