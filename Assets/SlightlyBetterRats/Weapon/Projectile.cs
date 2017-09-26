using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float launchSpeed;
    public float damage;
    public bool hitsTriggers;
    public bool hitsIfNotFired;
    public float linger = 0.5f;

    public Vector3 velocity { get; set; }
    public bool fired { get; private set; }

    public virtual void Fire() {
        Fire(transform.forward);
    }

    public virtual void Fire(Vector3 direction) {
        velocity = direction.normalized * launchSpeed;
        fired = true;
    }

	protected virtual void OnHitObject(Collider col, Vector3 position, Vector3 normal) {
        if (fired || hitsIfNotFired) {
            if (hitsTriggers || !col.isTrigger) {
                Health d = col.GetComponent<Health>();

                if (!d) {
                    d = col.GetComponentInParent<Health>();
                }

                if (d) {
                    d.ApplyDamage(new Damage(damage, position, normal));
                }

                velocity = Vector3.zero;
                Destroy(gameObject, linger);
            }
        }
    }
}
