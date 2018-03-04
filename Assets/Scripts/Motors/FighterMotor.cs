using SBR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterMotor : BasicMotor<FighterChannels> {
    public Rigidbody body { get; private set; }

    public float thrust;
    public float maxSpeed;

    public float angularThrust;
    public float maxRotSpeed;

    private Vector3 vel, angVel;
    
    protected override void Start() {
        base.Start();

        body = GetComponent<Rigidbody>();
        Time.fixedDeltaTime = 1.0f / 60;
    }

    public override void TakeInput() {
        vel = channels.thrust * transform.forward;
        angVel = channels.rotation;
    }

    private void FixedUpdate() {
        body.velocity = Vector3.MoveTowards(body.velocity, vel * maxSpeed, thrust * Time.deltaTime * Mathf.Abs(channels.thrust));

        body.angularVelocity = Vector3.MoveTowards(body.angularVelocity, angVel * maxRotSpeed, angularThrust * Time.deltaTime);
    }
}
