using SBR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFighterController : DecisionTree<FighterChannels> {
    public Fighter target;
    public Projectile projectile;
    public float boxCastDistance = 10;

    public BoxCollider box { get; private set; }
    public Fighter self { get; private set; }
    private RaycastHit wallHit;

    protected override void Start() {
        base.Start();

        self = GetComponent<Fighter>();
        box = GetComponent<BoxCollider>();

        root =
        BoolTest(Test_CollisionAhead,
            Action_AvoidWall,
            BoolTest(Test_InEnemyRange,
                Action_FlyBehind,
                BoolTest(Test_InMyRange,
                    BoolTest(Test_TooFar,
                        Action_Pursue,
                        BoolTest(Test_TooClose,
                            Action_Reverse,
                            Action_Strafe
                        )
                    ),
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

    private bool Test_CollisionAhead() {
        if (Physics.BoxCast(transform.position, box.size * 0.5f, self.body.velocity, out wallHit, transform.rotation, boxCastDistance * self.body.velocity.magnitude)) {
            return true; // I know
        }

        return false;
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
        dir = dir.normalized;
        Debug.DrawLine(transform.position, transform.position + dir * 20);

        Vector3 c = Vector3.Cross(transform.forward, dir) * 10;
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

    private void FireAtTarget() {
        Vector3 aim;

        if (SpaceUtil.PredictPosition(target.body, self.body, projectile.launchSpeed, out aim)) {
            controlled.aim = transform.position + aim * 1000;
            controlled.firing = true;
        }
    }

    public void Action_AvoidWall() {
        Vector3 v = self.body.velocity.normalized + wallHit.normal;
        controlled.thrust = RotateTowards(v);
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
        
        controlled.thrust = RotateTowards(awayFromAim * 0.5f - awayFromEnemy);
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

        controlled.thrust = -RotateTowards(awayFromEnemy - awayFromAim);
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
        
        controlled.thrust = RotateTowards(awayFromAim);
    }

    public void Action_Strafe() {
        //print("Strafe");
        RotateTowards(target.transform.position - transform.position);

        FireAtTarget();
        controlled.thrust = 0;
    }

    public void Action_Pursue() {
        //print("Pursue");
        
        FireAtTarget();
        controlled.thrust = RotateTowards(target.transform.position - transform.position); ;
    }

    public void Action_Reverse() {
        //print("Reverse");
        RotateTowards(target.transform.position - transform.position);

        FireAtTarget();
        controlled.thrust = -1;
    }
}
