using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterMotor : BasicMotor<FighterProxy> {
    public Rigidbody body { get; private set; }

    public float thrust;
    public float maxSpeed;

    public float angularThrust;
    public float maxRotSpeed;

    protected override void Awake() {
        base.Awake();

        body = GetComponent<Rigidbody>();
    }

    public override void TakeInput() {
        body.velocity = Vector3.MoveTowards(body.velocity, control.thrust * transform.forward * maxSpeed, thrust * Time.deltaTime * Mathf.Abs(control.thrust));

        body.angularVelocity = Vector3.MoveTowards(body.angularVelocity, control.rotation * maxRotSpeed, angularThrust * Time.deltaTime);
    }
}
