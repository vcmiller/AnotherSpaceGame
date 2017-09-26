using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArm : BasicMotor<BasicControlProxy> {
    public bool useControlRotation = true;

    private Quaternion rot;

    private void LateUpdate() {
        if (useControlRotation && control) {
            transform.rotation = rot;
        }
    }

    public override void TakeInput() {
        rot = control.rotation;
    }
}
