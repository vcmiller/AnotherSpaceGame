using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 angles = transform.localEulerAngles;

        angles.x = Mathf.MoveTowardsAngle(angles.x, Input.GetAxis("LookVertical") * 10, Time.deltaTime * 5);
        angles.y = Mathf.MoveTowardsAngle(angles.y, Input.GetAxis("LookHorizontal") * 10, Time.deltaTime * 5);

        transform.localEulerAngles = angles;
    }
}
