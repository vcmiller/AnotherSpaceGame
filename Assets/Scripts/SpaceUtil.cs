using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpaceUtil {
    public static bool PredictPosition(Rigidbody body, Rigidbody reference, float projSpeed, out Vector3 predicted) {
        Vector3 pos = body.transform.position - reference.transform.position;
        Vector3 vel = body.velocity - reference.velocity;

        Vector3 posN = pos.normalized;
        Vector3 velN = vel.normalized;
        
        float a = (projSpeed * projSpeed) - vel.sqrMagnitude;
        float b = 2 * pos.magnitude * vel.magnitude * Vector3.Dot(posN, velN);
        float c = -pos.sqrMagnitude;

        float disc = b * b - 4 * a * c;

        if (disc < 0) {
            predicted = Vector3.zero;
            return false;
        } else {
            float root = Mathf.Sqrt(disc);

            float time = (-b + root) / (2 * a);

            Vector3 pred = pos + vel * time;
            predicted = pred + reference.position;
            return true;
        }

    }
}
