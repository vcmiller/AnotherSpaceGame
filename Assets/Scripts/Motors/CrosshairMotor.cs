using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairMotor : BasicMotor<FighterProxy> {
    public float movementScale = .1f;

    public RectTransform rect { get; private set; }

    protected override void Awake() {
        base.Awake();

        rect = GetComponent<RectTransform>();
    }

    public override void TakeInput() {
        rect.anchoredPosition = movementScale * control.crosshair;
    }

    public Vector3 worldPosition {
        get {
            Canvas canvas = GetComponentInParent<Canvas>();
            Vector3 localPos = movementScale * control.crosshair;

            return canvas.transform.TransformPoint(localPos);
        }
    }
}
