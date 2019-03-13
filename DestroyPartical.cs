using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPartical : MonoBehaviour {
    public float time = 1f;
	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, time);
	}	
}
