using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Listener_Cube : MonoBehaviour {
    private Transform pos;
	// Use this for initialization
	void Start () {

       pos = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = new Vector3(pos.position.x, 0, pos.position.z);

		
	}
}
