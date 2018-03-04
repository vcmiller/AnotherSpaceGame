using SBR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetProjector : BasicMotor<FighterChannels> {
    public Material material;
    public Healthbar bar;
    public Text info;
    public Text header;

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

            bar.target = channels.target.health ?? null;
            info.text = channels.target.title + "\n" + channels.target.affil;
            header.enabled = true;
        } else {
            bar.target = null;
            info.text = "";
            header.enabled = false;
        }
    }
}
