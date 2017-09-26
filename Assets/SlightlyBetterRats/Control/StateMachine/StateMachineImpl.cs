using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ControlProxy))]
public class StateMachineImpl<T> : MonoBehaviour where T : ControlProxy {
    public T control { get; private set; }

	protected virtual void Awake() {
        control = GetComponent<T>();
    }
}
