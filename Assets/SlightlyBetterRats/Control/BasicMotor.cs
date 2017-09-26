using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicMotor<T> : Motor where T : ControlProxy {
    public T control { get; private set; }

    protected override void Awake() {
        control = GetComponentInParent<T>();
    }
}
