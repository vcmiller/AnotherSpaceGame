using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAim : MonoBehaviour {
    public Rigidbody target;
    public GameObject projectile;

    public float cooldown;

    public CooldownTimer timer { get; private set; }
    public Rigidbody body { get; private set; }

    public Vector3 velocity;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
        timer = new CooldownTimer(cooldown);
	}
	
	// Update is called once per frame
	void Update () {
        body.velocity = velocity;

        if (projectile && timer.Use()) {
            Projectile proj = Instantiate(projectile, transform.position, transform.rotation).GetComponent<Projectile>();

            Vector3 dir;
            if (SpaceUtil.PredictPosition(target, body, proj.launchSpeed, out dir)) {
                proj.Fire(dir);
                proj.velocity += body.velocity;
            }
        }

	}
}
