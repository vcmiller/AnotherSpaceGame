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

    protected override void Start() {
        base.Start();

        body = GetComponent<Rigidbody>();
    }

    public override void TakeInput() {
        body.velocity = Vector3.MoveTowards(body.velocity, channels.thrust * transform.forward * maxSpeed, thrust * Time.deltaTime * Mathf.Abs(channels.thrust));

        body.angularVelocity = Vector3.MoveTowards(body.angularVelocity, channels.rotation * maxRotSpeed, angularThrust * Time.deltaTime);
    }
}
