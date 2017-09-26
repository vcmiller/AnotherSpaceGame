using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCharacterController2D : PlayerController<CharacterProxy> {
	
	public void Axis_Horizontal(float value) {
        Vector3 right = viewTarget.transform.right;
        right.y = 0;
        right = right.normalized;

        controlled.movement += right * value;
    }

    public void ButtonDown_Jump() {
        controlled.jump = true;
    }
}
