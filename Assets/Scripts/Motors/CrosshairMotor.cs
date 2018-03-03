using SBR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairMotor : BasicMotor<FighterChannels> {
    public float movementScale = .1f;

    public RectTransform rect { get; private set; }

    protected override void Start() {
        base.Start();

        rect = GetComponent<RectTransform>();
    }

    public override void TakeInput() {
        rect.anchoredPosition = movementScale * channels.crosshair;
    }

    public Vector3 worldPosition {
        get {
            Canvas canvas = GetComponentInParent<Canvas>();
            Vector3 localPos = movementScale * channels.crosshair;

            return canvas.transform.TransformPoint(localPos);
        }
    }
}
