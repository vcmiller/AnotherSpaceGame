using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public abstract class CruiserSMBase : StateMachine {
    new protected CruiserChannels channels { get; private set; }

    public override void Initialize() {
        channels = GetComponent<Brain>().channels as CruiserChannels;

        base.Initialize();
    }

    protected void MoveInDirection(Vector3 dir) {

    }

    protected float RotateTowards(Vector3 dir) {
        dir = dir.normalized;

        Vector3 c = Vector3.Cross(transform.forward, dir) * 10;
        float d = Vector3.Dot(transform.forward, dir);

        float sqr = c.sqrMagnitude;

        if (sqr == 0) {
            c = transform.right;
        } else if (sqr > 1 || d < 0) {
            c = c.normalized;
        }

        channels.rotation = c;

        return Vector3.Dot(transform.forward, dir.normalized);
    }
}
