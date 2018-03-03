using SBR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetProjector : BasicMotor<FighterChannels> {
    public Material material;

    private int layer;
    public float size = 0.1f;

    public MeshRenderer mesh { get; private set; }

    protected override void Start() {
        base.Start();

        layer = LayerMask.NameToLayer("Default");
        mesh = GetComponent<MeshRenderer>();
    }

    public override void TakeInput() {

        if (channels.target) {
            transform.rotation = channels.target.transform.rotation;

            Mesh targetMesh = channels.target.GetComponent<MeshFilter>().mesh;

            Bounds b = targetMesh.bounds;
            float f = Mathf.Max(b.size.x, b.size.y, b.size.z);
            float scale = size / f;
            transform.localScale = new Vector3(scale, scale, scale);

            Graphics.DrawMesh(targetMesh, transform.localToWorldMatrix, material, layer);
        }
    }
}
