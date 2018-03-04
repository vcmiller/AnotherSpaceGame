using UnityEngine;
using SBR;

public class FighterChannels : SBR.Channels {
    public FighterChannels() {
        RegisterInputChannel("thrust", 0f, false);
        RegisterInputChannel("rotation", new Vector3(0, 0, 0), true);
        RegisterInputChannel("crosshair", new Vector3(0, 0, 0), false);
        RegisterInputChannel("aim", new Vector3(0, 0, 0), false);
        RegisterInputChannel("firing", false, true);
        RegisterInputChannel("target", null, false);

    }
    

    public float thrust {
        get {
            return GetInput<float>("thrust");
        }

        set {
            SetFloat("thrust", value, -1, 1);
        }
    }

    public Vector3 rotation {
        get {
            return GetInput<Vector3>("rotation");
        }

        set {
            SetVector("rotation", value);
        }
    }

    public Vector3 crosshair {
        get {
            return GetInput<Vector3>("crosshair");
        }

        set {
            SetVector("crosshair", value);
        }
    }

    public Vector3 aim {
        get {
            return GetInput<Vector3>("aim");
        }

        set {
            SetVector("aim", value);
        }
    }

    public bool firing {
        get {
            return GetInput<bool>("firing");
        }

        set {
            SetInput("firing", value);
        }
    }

    public TargetableObject target {
        get {
            return GetInput<TargetableObject>("target");
        }

        set {
            SetInput("target", value);
        }
    }

}
