using UnityEngine;
using System.Collections;

public class Brush : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(GetComponent<BrushControl>());
        Destroy(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
