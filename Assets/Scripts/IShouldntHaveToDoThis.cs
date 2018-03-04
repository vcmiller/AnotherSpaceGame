using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IShouldntHaveToDoThis : MonoBehaviour {
    
	
	// Update is called once per frame
	void Update () {
        var g = GetComponent<VerticalLayoutGroup>();
        g.enabled = false;
        g.enabled = true;
    }
}
