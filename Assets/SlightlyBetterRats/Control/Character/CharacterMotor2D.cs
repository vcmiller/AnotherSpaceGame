using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Motor class for a humanoid character. Enables movement and collision without the use of a Rigidbody.
 * However, if you want to get trigger/collision events for objects that don't have a Rigidbody, you
 * need to put a Rigidbody on this object and make it kinematic.
 */
[RequireComponent(typeof(BoxCollider2D))]
public class CharacterMotor2D : BasicMotor<CharacterProxy> {
    public BoxCollider2D box { get; private set; }

    public bool grounded { get; private set; }

    [HideInInspector]
    public Vector2 velocity;

    [HideInInspector]
    private bool wasInAir = false;

    [Header("General")]
    [Tooltip("Increase this if your character is passing through colliders. Should be about 0.05 * the character's height.")]
    public float queryExtraDistance = 0.1f;

    [Tooltip("Layers that block the character.")]
    public LayerMask[] blockingLayers = new LayerMask[1];

    [Header("Movement: Walking")]
    [Tooltip("The max walk speed of the character.")]
    public float walkSpeed = 5;

    [Tooltip("The walking (ground) acceleration of the character.")]
    public float walkAcceleration = 5;

    [Tooltip("The maximum slope, in degrees, that the character can climb.")]
    public float maxSlope = 45;

    [Tooltip("The maximum height that the character can step up onto.")]
    public float maxStep = 0.2f;

    [Header("Jumping")]
    [Tooltip("The speed at which the character jumps.")]
    public float jumpSpeed = 4;

    [Tooltip("The value to multiply Physics.Gravity by.")]
    public float gravityScale = 1;

    [Header("Movement: Falling")]
    [Tooltip("Air control multiplier (air acceleration is Air Control * Walk Acceleration.")]
    public float airControl = 0.5f;

    protected override void Awake() {
        base.Awake();

        box = GetComponent<BoxCollider2D>();
    }

    public override void TakeInput() {
        bool collider = box.enabled;
        bool startIn = Physics2D.queriesStartInColliders;
        bool triggers = Physics2D.queriesHitTriggers;
        box.enabled = false;
        Physics2D.queriesHitTriggers = false;
        Physics2D.queriesStartInColliders = false;

        int queryMask = 0;
        foreach (LayerMask lm in blockingLayers) {
            queryMask |= lm.value;
        }

        Vector2 move = control.movement;
        move.y = 0;
        move *= walkSpeed;

        //if (body.isGrounded) {
        float accel = walkAcceleration;
        if (!grounded) {
            accel *= airControl;
        }
        velocity = Vector2.MoveTowards(velocity, new Vector2(move.x, velocity.y), accel * Time.deltaTime);
        velocity += Physics2D.gravity * gravityScale * Time.deltaTime;

        if (grounded && control.jump) {
            velocity.y = jumpSpeed;
        }

        RaycastHit2D hit;

        Vector2 center = box.transform.TransformPoint(box.offset);
        Vector2 size = new Vector2(box.size.x * box.transform.lossyScale.x, box.size.y * box.transform.lossyScale.y);
        float angle = box.transform.eulerAngles.z;

        Vector2 movement = velocity * Time.deltaTime;
        movement = Vector3.ProjectOnPlane(movement, transform.up);
        Vector2 moveDir = movement.normalized;
        float d = movement.magnitude;

        if (d > 0 && (hit = Physics2D.BoxCast(center - moveDir * queryExtraDistance, size, angle, movement, d + queryExtraDistance, queryMask))) {
            Vector2 norm = hit.normal;
            float slope = Vector2.Angle(hit.normal, transform.up);

            bool slopeOK = true;

            if (slope > maxSlope) {
                norm = Vector3.ProjectOnPlane(norm, transform.up).normalized;
                slopeOK = false;

            } // Else climb slope

            // Try to go up step
            RaycastHit2D stepHit;
            float stepHeight = (maxStep + queryExtraDistance);
            Vector2 stepTestOff = movement + (Vector2)transform.up * stepHeight;

            bool step = false;

            if (!slopeOK) {
                if (stepHit = Physics2D.BoxCast(center + stepTestOff, size, angle, -transform.up, stepHeight, queryMask)) {
                    stepHeight -= stepHit.distance;

                    Vector2 stepHeightOff = (Vector2)transform.up * (stepHeight + queryExtraDistance) - moveDir * queryExtraDistance;

                    if (stepHeight <= maxStep) {
                        slope = Vector2.Angle(stepHit.normal, transform.up);
 
                        if (slope < maxSlope && !Physics2D.BoxCast(center + stepHeightOff, size, angle, movement, d + queryExtraDistance, queryMask)) {
                            step = true;
                            movement += (Vector2)transform.up * stepHeight;
                        }
                    }
                }
            }

            if (!step) {
                float dRem = (d + queryExtraDistance) - hit.distance;
                Vector2 badMovement = movement.normalized * dRem;
                Vector2 comp = Vector3.Project(-badMovement, norm);

                movement += comp;

                comp.y = 0;

                velocity += comp / Time.deltaTime;
            }
        }

        transform.Translate(movement, Space.World);

        Vector2 vert = velocity * Time.deltaTime;
        vert = Vector3.Project(vert, transform.up);
        d = vert.magnitude;
        Vector2 dir = vert.normalized;
        grounded = false;
        
        if (d > 0 && (hit = Physics2D.BoxCast(center - dir * queryExtraDistance, size, angle, vert, d + queryExtraDistance, queryMask))) {
            Vector2 norm = hit.normal;

            float slope = Vector2.Angle(hit.normal, transform.up);

            if (slope < maxSlope) {
                norm = Vector3.Project(norm, transform.up).normalized;
                grounded = true;
            }

            float dRem = (d + queryExtraDistance) - hit.distance;
            Vector2 badMovement = vert.normalized * dRem;
            Vector2 comp = Vector3.Project(-badMovement, norm);

            vert += comp;

            //comp.x = 0;

            if (velocity.y + comp.y > 0) {
                comp.y = Mathf.Max(-velocity.y, 0);
            }

            velocity += comp / Time.deltaTime;
        }

        transform.Translate(vert, Space.World);

        box.enabled = collider;
        Physics2D.queriesHitTriggers = triggers;
        Physics2D.queriesStartInColliders = startIn;

        //wasInAir = false;
        /*} else {
            velocity = Vector3.MoveTowards(velocity, new Vector3(move.x, velocity.y, move.z), walkAcceleration * airControl * Time.deltaTime);

            velocity += Physics.gravity * Time.deltaTime;

            wasInAir = true;
        }

        var flags = body.Move(velocity * Time.deltaTime);*/

    }
}
