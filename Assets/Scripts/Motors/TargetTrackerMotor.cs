using SBR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetTrackerMotor : BasicMotor<FighterChannels> {

    public RectTransform rect { get; private set; }
    public Image image { get; private set; }
    public RectTransform canvas { get; private set; }
    public Rigidbody body { get; private set; }
    public Camera cam { get; private set; }

    public Projectile proj;

    protected override void Start() {
        base.Start();

        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        body = GetComponentInParent<Rigidbody>();
        cam = transform.root.GetComponentInChildren<Camera>();
    }

    public override void TakeInput() {
        if (channels.target) {
            float d;
            Plane canvasPlane = new Plane(canvas.transform.forward, canvas.transform.position);

            Vector3 aimDir;
            bool canPredict = SpaceUtil.PredictPosition(channels.target.body, body, proj.launchSpeed, out aimDir);

            if (canPredict) {
                Ray ray = new Ray(cam.transform.position, aimDir);
                bool b = canvasPlane.Raycast(ray, out d);

                if (b) {
                    transform.position = ray.GetPoint(d);
                }
            }

            Vector2 s = canvas.sizeDelta;

            Vector2 v = rect.anchoredPosition + (s / 2);

            image.enabled = v.x > 0 && v.y > 0 && v.x < s.x && v.y < s.y;
        } else {
            image.enabled = false;
        }
    }
}
