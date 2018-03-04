using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBR {
    public class Projectile : MonoBehaviour {
        public float launchSpeed;
        public float damage;
        public bool hitsTriggers;
        public bool hitsIfNotFired;
        public float linger = 0.5f;
        public bool destroyOnHit = true;
        public GameObject impactPrefab;

        public Vector3 velocity { get; set; }
        public bool fired { get; private set; }

        public virtual void Fire() {
            Fire(transform.forward);
        }

        public virtual void Fire(Vector3 direction, bool align = true) {
            velocity = direction.normalized * launchSpeed;
            if (align) {
                transform.forward = direction;
            }
            fired = true;
        }

        protected virtual void OnHitCollider2D(Collider2D col, Vector2 position, Vector2 normal) {
            if (fired || hitsIfNotFired) {
                if (hitsTriggers || !col.isTrigger) {
                    OnHitObject(col.transform, position, normal);
                }
            }
        }

        protected virtual void OnHitCollider(Collider col, Vector3 position, Vector3 normal) {
            if (fired || hitsIfNotFired) {
                if (hitsTriggers || !col.isTrigger) {
                    OnHitObject(col.transform, position, normal);
                }
            }
        }

        protected virtual void OnHitObject(Transform col, Vector3 position, Vector3 normal) {
            Health d = col.GetComponent<Health>();

            if (!d) {
                d = col.GetComponentInParent<Health>();
            }

            if (d) {
                d.Damage(new Damage(damage, position, normal));
            }

            velocity = Vector3.zero;
            transform.position = position;

            if (destroyOnHit) {
                Destroy(gameObject, linger);
            }

            if (impactPrefab) {
                Instantiate(impactPrefab, position, transform.rotation);
            }
        }
    }
}