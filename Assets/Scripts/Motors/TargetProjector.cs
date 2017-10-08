using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetProjector : BasicMotor<FighterProxy> {
    
    public Material material;

    public MeshFilter target;

    private int layer;
    public float size = 0.1f;

    protected override void Awake() {
        base.Awake();

        layer = LayerMask.NameToLayer("Default");
    }

    public override void TakeInput() {
        transform.rotation = target.transform.rotation;

        Bounds b = target.mesh.bounds;
        float f = Mathf.Max(b.size.x, b.size.y, b.size.z);
        float scale = size / f;
        transform.localScale = new Vector3(scale, scale, scale);

        Graphics.DrawMesh(target.mesh, transform.localToWorldMatrix, material, layer);
    }
}
