using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicControlProxy : ControlProxy {
    protected override void Awake() {
        base.Awake();

        RegisterInputChannel("Movement", Vector3.zero, true);
        RegisterInputChannel("Rotation", Quaternion.identity, false);
    }

    public Vector3 movement {
        get {
            return GetVector("Movement");
        }

        set {
            SetVector("Movement", value, 1);
        }
    }

    public Quaternion rotation {
        get {
            return GetQuaternion("Rotation");
        }

        set {
            SetQuaternion("Rotation", value);
        }
    }
}
