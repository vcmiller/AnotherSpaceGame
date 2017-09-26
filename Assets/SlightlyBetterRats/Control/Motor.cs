using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Motor : MonoBehaviour {
    protected virtual void Awake() {

    }

    public abstract void TakeInput();
}
