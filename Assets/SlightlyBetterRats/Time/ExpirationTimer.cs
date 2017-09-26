
using UnityEngine;

public class ExpirationTimer {
	public float expiration { get; set; }
	public float lastSet { get; set; }

	public bool expired {
		get {
			return Time.time - lastSet > expiration;
		}
	}

    public float remaining {
        get {
            return Mathf.Max(0, expiration - (Time.time - lastSet));
        }
    }

    public float remainingRatio {
        get {
            if (expired) {
                return 0;
            } else {
                return 1 - ((Time.time - lastSet) / expiration);
            }
        }
    }

	public ExpirationTimer (float expiration) {
		this.expiration = expiration;
		Clear ();
	}

	public void Set() {
		lastSet = Time.time;
	}

	public void Clear() {
		lastSet = Time.time - expiration;
	}
}

