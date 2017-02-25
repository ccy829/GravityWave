using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGravity : MonoBehaviour {
    public float speed;
    float g;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        g = Mathf.Sin(Time.time*speed);
        GameManager.GetInstance().gravity = g;
	}
}
