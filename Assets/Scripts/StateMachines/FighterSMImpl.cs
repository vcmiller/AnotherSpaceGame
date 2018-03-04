using UnityEngine;
using SBR;

public class FighterSMImpl : FighterSM {
    public TargetableObject initTarget;
    public float boxCastDistance = 10;
    public float desiredTargetDistance = 75;

    public BoxCollider box { get; private set; }
    public Fighter self { get; private set; }
    new public FighterChannels channels { get; private set; }

    private RaycastHit wallHit;
    private Projectile projectile;

    private bool collisionAhead {
        get {
            return Physics.BoxCast(transform.position, box.size * 0.5f, self.body.velocity, out wallHit, transform.rotation, boxCastDistance * self.body.velocity.magnitude);
        }
    }

    private Vector3 enemyAim {
        get {
            return channels.target.transform.forward;
        }
    }

    private Vector3 enemyPos {
        get {
            return channels.target.transform.position;
        }
    }

    private Vector3 toEnemy {
        get {
            return (enemyPos - transform.position).normalized;
        }
    }

    private Vector3 fromEnemy {
        get {
            return -toEnemy;
        }
    }

    private float myAttackPotential {
        get {
            return (Vector3.Dot(transform.forward, toEnemy) + 1) / 2;
        }
    }

    private float enemyAttackPotential {
        get {
            return (Vector3.Dot(enemyAim, fromEnemy) + 1) / 2;
        }
    }

    private Vector3 awayFromEnemyAim {
        get {
            Vector3 f = fromEnemy;
            Vector3 c = Vector3.Cross(enemyAim, f);

            if (c.sqrMagnitude == 0) {
                c = transform.right;
            }

            return Vector3.Cross(c, f).normalized;
        }
    }

    public override void Initialize() {
        self = GetComponent<Fighter>();
        box = GetComponent<BoxCollider>();
        channels = GetComponent<Brain>().channels as FighterChannels;

        channels.target = initTarget;

        projectile = GetComponentInChildren<FighterWeapons>().bulletPrefab.GetComponent<Projectile>();

        base.Initialize();
    }

    public override void GetInput() {
        if (collisionAhead) {
            state = StateID.AvoidWall;
        }

        base.GetInput();
    }

    private float RotateTowards(Vector3 dir) {
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

    private void FireAtTarget() {
        Vector3 aim;

        if (SpaceUtil.PredictPosition(channels.target.body, self.body, projectile.launchSpeed, out aim)) {
            channels.aim = transform.position + aim * 1000;
            channels.firing = true;
        }
    }

    protected override void State_AvoidWall() {
        Vector3 v = self.body.velocity.normalized + wallHit.normal;
        channels.thrust = RotateTowards(v);
    }

    protected override void State_Pursue() {
        FireAtTarget();
        RotateTowards(channels.target.transform.position - transform.position);

        channels.thrust = Vector3.Distance(transform.position, enemyPos) - desiredTargetDistance;
    }

    protected override void State_Evade() {
        channels.thrust = RotateTowards(awayFromEnemyAim);
    }

    protected override void State_Reversal() {
        channels.thrust = RotateTowards(awayFromEnemyAim * 0.5f - fromEnemy);
    }

    protected override bool TransitionCond_AvoidWall_Pursue() {
        return !collisionAhead;
    }

    protected override bool TransitionCond_Pursue_Evade() {
        return enemyAttackPotential > 0.9f;
    }

    protected override bool TransitionCond_Evade_Reversal() {
        return enemyAttackPotential < 0.8f;
    }

    protected override bool TransitionCond_Reversal_Pursue() {
        return enemyAttackPotential < 0.6f;
    }
}
