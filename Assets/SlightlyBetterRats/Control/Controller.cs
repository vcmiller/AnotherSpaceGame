using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour {
    protected virtual void Start() { }

    public abstract void GetInput();
}
