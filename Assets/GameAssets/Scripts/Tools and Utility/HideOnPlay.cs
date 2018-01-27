using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnPlay : MonoBehaviour {
    public float delay = 0f;
	// Use this for initialization
	void Start () {
        Destroy(GetComponent<Renderer>(), delay);
        Destroy(GetComponent<MeshFilter>(), delay);
    }
}
