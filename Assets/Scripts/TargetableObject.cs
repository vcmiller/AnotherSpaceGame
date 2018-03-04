using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public class TargetableObject : MonoBehaviour {
    public string title;
    public string affil;

    public Rigidbody body { get; private set; }
    public Health health { get; private set; }

    protected virtual void Awake() {
        body = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
    }

    private void OnZeroHealth() {
        Destroy(gameObject);
    }
}
