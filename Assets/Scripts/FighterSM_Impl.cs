using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSM_Impl : StateMachineImpl<FighterProxy> {
    public Fighter target { get; private set; }

    protected override void Awake() {
        base.Awake();

        foreach (Fighter fighter in FindObjectsOfType<Fighter>()) {
            if (fighter.transform != transform) {
                target = fighter;
                break;
            }
        }
    }

    private float myAttack {
        get {
            return (Vector3.Dot(transform.forward, (target.transform.position - transform.position).normalized) + 1) / 2;
        }
    }

    private float enemyAttack {
        get {
            return (Vector3.Dot(target.transform.forward, (transform.position - target.transform.position).normalized) + 1) / 2;
        }
    }

    private float RotateTowards(Vector3 dir) {
        Vector3 c = Vector3.Cross(transform.forward, dir);

        if (c.sqrMagnitude > 0) {
            c = c.normalized;
        } else {
            c = transform.right;
        }

        control.rotation = c;

        return Vector3.Dot(transform.forward, dir.normalized);
    }

    public void State_Reversal() {
        Vector3 enemyAim = target.transform.forward;
        Vector3 awayFromEnemy = (transform.position - target.transform.position).normalized;

        Vector3 c = Vector3.Cross(enemyAim, awayFromEnemy);

        if (c.sqrMagnitude == 0) {
            c = transform.right;
        }

        Vector3 awayFromAim = Vector3.Cross(c, awayFromEnemy).normalized;

        RotateTowards(awayFromAim - awayFromEnemy);
        control.thrust = 1.0f;
    }

    public void State_Evade() {
        Vector3 enemyAim = target.transform.forward;
        Vector3 awayFromEnemy = (transform.position - target.transform.position).normalized;

        Vector3 c = Vector3.Cross(enemyAim, awayFromEnemy);

        if (c.sqrMagnitude == 0) {
            c = transform.right;
        }

        Vector3 awayFromAim = Vector3.Cross(c, awayFromEnemy).normalized;

        RotateTowards(awayFromAim);
        control.thrust = 1.0f;
    }

    public void State_Strafe() {
        RotateTowards(target.transform.position - transform.position);

        control.target = target.transform.position;
        control.firing = myAttack > 0.9f;
        control.thrust = 0;
    }

    public void State_Pursue() {
        RotateTowards(target.transform.position - transform.position);

        control.target = target.transform.position;
        control.firing = myAttack > 0.9f;
        control.thrust = 1;
    }

    public void State_Drop() {
        Vector3 enemyAim = target.transform.forward;
        Vector3 awayFromEnemy = (transform.position - target.transform.position).normalized;

        Vector3 c = Vector3.Cross(enemyAim, awayFromEnemy);

        if (c.sqrMagnitude == 0) {
            c = transform.right;
        }

        Vector3 awayFromAim = Vector3.Cross(c, awayFromEnemy).normalized;

        RotateTowards(awayFromEnemy - awayFromAim * 0.25f);
        control.thrust = -1.0f;
    }
}
