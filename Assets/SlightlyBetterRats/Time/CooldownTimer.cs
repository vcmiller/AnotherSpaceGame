
using System;
using UnityEngine;

public class CooldownTimer {
	public float cooldown { get; set; }
	public float lastUse { get; set; }

    public float chargeRatio {
        get {
            if (Time.time - lastUse > cooldown) {
                return 1.0f;
            } else {
                return (Time.time - lastUse) / cooldown;
            }
        }
    }

    public bool canUse {
        get {
            return Time.time - lastUse > cooldown;
        }
    }

    public CooldownTimer (float cooldown) {
		this.cooldown = cooldown;
		lastUse = Time.time;
    }

    public bool Use() {
        if (Time.time - lastUse > cooldown) {
            lastUse = Time.time;
            return true;
        } else {
            return false;
        }
    }

    public void Clear() {
		lastUse = Time.time - cooldown;
	}

    public void Reset() {
        lastUse = Time.time;
    }
}

