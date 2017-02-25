using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBillBoard : MonoBehaviour {
    public GameObject up;
    public GameObject down;
    bool isUp;
    float gravity;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gravity = GameManager.GetInstance().gravity;
        isUp = gravity > 0;
        if (Mathf.Abs(gravity) < 0.1f)
        {
            up.SetActive(false);
            down.SetActive(false);
        }
        else
        {
            up.SetActive(isUp);
            down.SetActive(!isUp);
        }

	}

    
}
