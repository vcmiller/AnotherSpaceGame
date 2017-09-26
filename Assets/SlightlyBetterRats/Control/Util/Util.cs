using UnityEngine;
using System.Collections;

public static class Util {

    public static void GetPoints(this CapsuleCollider capsule, out Vector3 point1, out Vector3 point2, out float radius, out float height) {
        Vector3 capsuleCenter = capsule.transform.TransformPoint(capsule.center);
        Vector3 capsuleUp;
        float scaleY;
        float scaleXZ;
        
        if (capsule.direction == 0) {
            capsuleUp = capsule.transform.right;
            scaleY = capsule.transform.lossyScale.x;
            scaleXZ = Mathf.Max(Mathf.Abs(capsule.transform.localScale.y), Mathf.Abs(capsule.transform.localScale.z));
        } else if (capsule.direction == 1) {
            capsuleUp = capsule.transform.up;
            scaleY = capsule.transform.lossyScale.y;
            scaleXZ = Mathf.Max(Mathf.Abs(capsule.transform.localScale.x), Mathf.Abs(capsule.transform.localScale.z));
        } else {
            capsuleUp = capsule.transform.forward;
            scaleY = capsule.transform.lossyScale.z;
            scaleXZ = Mathf.Max(Mathf.Abs(capsule.transform.localScale.x), Mathf.Abs(capsule.transform.localScale.y));
        }

        radius = scaleXZ * capsule.radius;

        height = Mathf.Max(scaleY * capsule.height, radius * 2);

        float h = height / 2 - radius;

        point1 = capsule.transform.position + capsuleUp * h;
        point2 = capsule.transform.position - capsuleUp * h;
    }
}
