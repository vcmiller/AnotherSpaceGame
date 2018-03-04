using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SBR;

public class FighterWeapons : BasicMotor<FighterChannels> {
    public GameObject bulletPrefab;
    public float fireCooldown;
    public int clipSize;
    public float reloadTime;

    public CooldownTimer fireTimer { get; private set; }
    public Magazine clip { get; private set; }

    protected override void Start() {
        base.Start();

        fireTimer = new CooldownTimer(fireCooldown);
        clip = new Magazine(clipSize, reloadTime);
    }

    public override void TakeInput() {
        if (channels.firing && fireTimer.canUse && clip.canFire) {

            if (Vector3.Dot(transform.forward, (channels.aim - transform.position).normalized) > 0.8) {
                fireTimer.Use();
                clip.Fire();

                GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

                var p = bullet.GetComponent<DontHitMe>();

                p.transform.forward = channels.aim - bullet.transform.position;
                p.creator = transform.root;
                p.Fire();
                p.velocity += GetComponentInParent<Rigidbody>().velocity;
            }

        }
    }
}
