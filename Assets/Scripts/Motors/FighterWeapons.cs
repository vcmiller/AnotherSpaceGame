using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterWeapons : BasicMotor<FighterProxy> {
    public GameObject bulletPrefab;
    public float fireCooldown;
    public int clipSize;
    public float reloadTime;

    public CooldownTimer fireTimer { get; private set; }
    public Magazine clip { get; private set; }

    protected override void Awake() {
        base.Awake();

        fireTimer = new CooldownTimer(fireCooldown);
        clip = new Magazine(clipSize, reloadTime);
    }

    public override void TakeInput() {
        if (control.firing && fireTimer.canUse && clip.canFire) {

            if (Vector3.Dot(control.transform.forward, (control.target - control.transform.position).normalized) > 0.8) {
                fireTimer.Use();
                clip.Fire();

                GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

                var p = bullet.GetComponent<Projectile>();

                p.transform.forward = control.target - bullet.transform.position;
                p.creator = transform.root.gameObject;
                p.Fire();
                p.velocity += GetComponentInParent<Rigidbody>().velocity;
            }

        }
    }
}
