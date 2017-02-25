using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveByTime : MonoBehaviour {
    public float time;
    float timer;
    bool actived = false;
    public GameObject needActive;
	// Use this for initialization
	void Start () {
        needActive.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (!actived)
        {
            timer += Time.deltaTime;
            if (timer >= time)
            {
                actived = true;
                
                needActive.SetActive(true);
            }
        }
		
	}
}
