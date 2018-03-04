using UnityEngine;
using SBR;

public class CruiserChannels : SBR.Channels {
    public CruiserChannels() {
        RegisterInputChannel("thrust", 0f, false);
        RegisterInputChannel("rotation", new Vector3(0, 0, 0), true);

    }
    

    public float thrust {
        get {
            return GetInput<float>("thrust");
        }

        set {
            SetFloat("thrust", value);
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

}
