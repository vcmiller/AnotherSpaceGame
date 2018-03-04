using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBR {
    public class PointProjectile : Projectile {
        public LayerMask hitMask;

        private void Update() {
            Vector3 oldPosition = transform.position;
            transform.position += velocity * Time.deltaTime;

            RaycastHit hit;

            bool trig = Physics.queriesHitTriggers;
            Physics.queriesHitTriggers = hitsTriggers;

            if (Physics.Linecast(oldPosition, transform.position, out hit, hitMask)) {
                OnHitCollider(hit.collider, hit.point, hit.normal);
            }

            Physics.queriesHitTriggers = trig;
        }
    }
}