using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBR {
    public class PointProjectile2D : Projectile {
        public LayerMask hitMask;

        private void Update() {
            Vector3 oldPosition = transform.position;
            transform.position += velocity * Time.deltaTime;

            RaycastHit2D hit;

            bool trig = Physics2D.queriesHitTriggers;
            Physics2D.queriesHitTriggers = hitsTriggers;

            if (hit = Physics2D.Linecast(oldPosition, transform.position, hitMask)) {
                OnHitCollider2D(hit.collider, hit.point, hit.normal);
            }

            Physics2D.queriesHitTriggers = trig;
        }
    }
}