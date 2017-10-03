using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFighterController : DecisionTree<FighterProxy> {
    public Fighter target;

    protected override void Start() {
        base.Start();
        root = 
        BoolTest(Test_InEnemyRange,
            BoolTest(Test_FacingTowards,
                Action_FlyBehind,
                Action_Drop
            ),
            BoolTest(Test_TooFar,
                Action_Pursue,
                BoolTest(Test_TooClose,
                    Action_Reverse,
                    Action_Strafe
                )
            )
        );
    }

    private bool Test_TooFar() {
        return Vector3.Distance(transform.position, target.transform.position) > 100;
    }

    private bool Test_TooClose() {
        return Vector3.Distance(transform.position, target.transform.position) < 50;
    }

    private bool Test_FacingTowards() {
        return myAttack > 0.7f;
    }

    private bool Test_InMyRange() {
        return myAttack > 0.9f;
    }

    private bool Test_InEnemyRange() {
        return enemyAttack > 0.9f;
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
        float d = Vector3.Dot(transform.forward, dir);

        float sqr = c.sqrMagnitude;

        if (sqr == 0) {
            c = transform.right;
        } else if (sqr > 1 || d < 0) {
            c = c.normalized;
        }

        controlled.rotation = c;

        return Vector3.Dot(transform.forward, dir.normalized);
    }

    public void Action_FlyBehind() {
        //print("FlyBehind");
        Vector3 enemyAim = target.transform.forward;
        Vector3 awayFromEnemy = (transform.position - target.transform.position).normalized;

        Vector3 c = Vector3.Cross(enemyAim, awayFromEnemy);

        if (c.sqrMagnitude == 0) {
            c = transform.right;
        }

        Vector3 awayFromAim = Vector3.Cross(c, awayFromEnemy).normalized;

        RotateTowards(awayFromAim * 0.5f - awayFromEnemy);
        controlled.thrust = 1.0f;
    }

    public void Action_Drop() {
        //print("Drop");
        Vector3 enemyAim = target.transform.forward;
        Vector3 awayFromEnemy = (transform.position - target.transform.position).normalized;

        Vector3 c = Vector3.Cross(enemyAim, awayFromEnemy);

        if (c.sqrMagnitude == 0) {
            c = transform.right;
        }

        Vector3 awayFromAim = Vector3.Cross(c, awayFromEnemy).normalized;

        RotateTowards(awayFromEnemy - awayFromAim * 0.2f);
        controlled.thrust = -1.0f;
    }

    public void Action_Evade() {
        //print("Evade");
        Vector3 enemyAim = target.transform.forward;
        Vector3 awayFromEnemy = (transform.position - target.transform.position).normalized;

        Vector3 c = Vector3.Cross(enemyAim, awayFromEnemy);

        if (c.sqrMagnitude == 0) {
            c = transform.right;
        }

        Vector3 awayFromAim = Vector3.Cross(c, awayFromEnemy).normalized;

        RotateTowards(awayFromAim);
        controlled.thrust = 1.0f;
    }

    public void Action_Strafe() {
        //print("Strafe");
        RotateTowards(target.transform.position - transform.position);

        controlled.target = target.transform.position;
        controlled.firing = myAttack > 0.9f;
        controlled.thrust = 0;
    }

    public void Action_Pursue() {
        //print("Pursue");
        RotateTowards(target.transform.position - transform.position);

        controlled.target = target.transform.position;
        controlled.firing = myAttack > 0.9f;
        controlled.thrust = 1;
    }

    public void Action_Reverse() {
        //print("Reverse");
        RotateTowards(target.transform.position - transform.position);

        controlled.target = target.transform.position;
        controlled.firing = myAttack > 0.9f;
        controlled.thrust = -1;
    }
}
