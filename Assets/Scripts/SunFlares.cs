using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFlares : MonoBehaviour {
	// Use this for initialization
	void Start () {
        var ps = GetComponentInChildren<ParticleSystem>();
        var p = ps.main;
        var m = GetComponentInChildren<MeshRenderer>().sharedMaterial;

        p.startColor = m.color;

        var particles = new ParticleSystem.Particle[ps.particleCount];
        ps.GetParticles(particles);

#pragma warning disable
        for (int i = 0; i < particles.Length; i++) {
            particles[i].color = m.color;
        }
#pragma warning restore

        ps.SetParticles(particles, particles.Length);
	}
}
