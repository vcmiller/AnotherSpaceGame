using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public class DontHitMe : PointProjectile {
    public Transform creator { get; set; }

    protected override void OnHitCollider(Collider col, Vector3 position, Vector3 normal) {
        if (col.transform.root != creator) {
            base.OnHitCollider(col, position, normal);
        }
    }
}
