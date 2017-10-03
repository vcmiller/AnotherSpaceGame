using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour {
    public Rigidbody body { get; private set; }

    private void Awake() {
        body = GetComponent<Rigidbody>();
    }

    private void ZeroHealth() {
        Destroy(gameObject);
    }
}
