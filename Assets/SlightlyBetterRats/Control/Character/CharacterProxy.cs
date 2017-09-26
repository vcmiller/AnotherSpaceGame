using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProxy : BasicControlProxy {
    protected override void Awake() {
        base.Awake();

        RegisterInputChannel("Jump", false, true);
    }

    public bool jump {
        get {
            return GetBool("Jump");
        }

        set {
            SetBool("Jump", value);
        }
    }
}
