using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterProxy : ControlProxy {
    public Vector2 crosshairLimits;

	// Use this for initialization
	void Start () {
        RegisterInputChannel("Thrust", 0.0f, false);
        RegisterInputChannel("Rotation", Vector3.zero, true);
        RegisterInputChannel("Crosshair", Vector3.zero, false);
        RegisterInputChannel("Target", Vector3.zero, false);
        RegisterInputChannel("Firing", false, true);
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

    public Vector2 crosshair {
        get {
            return GetVector("Crosshair");
        }

        set {
            float x = value.x;
            float y = value.y;

            x = Mathf.Clamp(x, -crosshairLimits.x, crosshairLimits.x);
            y = Mathf.Clamp(y, -crosshairLimits.y, crosshairLimits.y);

            SetVector("Crosshair", new Vector2(x, y));
        }
    }

    public Vector3 target {
        get {
            return GetVector("Target");
        }

        set {
            SetVector("Target", value);
        }
    }

    public bool firing {
        get {
            return GetBool("Firing");
        }

        set {
            SetBool("Firing", value);
        }
    }
}
