using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterProxy : ControlProxy {

	// Use this for initialization
	void Start () {
        RegisterInputChannel("Thrust", 0.0f, false);
        RegisterInputChannel("Rotation", Vector3.zero, true);
	}
	
	public float thrust {
        get {
            return GetFloat("Thrust");
        }

        set {
            SetFloat("Thrust", value, -1, 1);
        }
    }

    public Vector3 rotation {
        get {
            return GetVector("Rotation");
        }

        set {
            SetVector("Rotation", value, 1);
        }
    }
}
