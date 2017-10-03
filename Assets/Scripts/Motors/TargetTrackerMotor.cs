using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetTrackerMotor : BasicMotor<FighterProxy> {

    public RectTransform rect { get; private set; }
    public Image image { get; private set; }
    public RectTransform canvas { get; private set; }
    public Rigidbody body { get; private set; }

    public Camera cam;
    public Projectile proj;
    public Rigidbody target;

    protected override void Awake() {
        base.Awake();

        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        body = GetComponentInParent<Rigidbody>();
    }

    public override void TakeInput() {
        if (target) {
            float d;
            Plane canvasPlane = new Plane(canvas.transform.forward, canvas.transform.position);

            Vector3 aimDir;
            bool canPredict = SpaceUtil.PredictPosition(target, body, proj.launchSpeed, out aimDir);

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
