using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAhead : BasicMotor<FighterProxy> {

    public override void TakeInput() {
        Vector3 thrust = control.rotation;
        thrust = transform.InverseTransformVector(thrust);

        Vector3 angles = transform.localEulerAngles;

        angles.x = Mathf.MoveTowardsAngle(angles.x, thrust.x * 5, Time.deltaTime * 10);
        angles.y = Mathf.MoveTowardsAngle(angles.y, thrust.y * 5, Time.deltaTime * 10);

        transform.localEulerAngles = angles;
    }
}
