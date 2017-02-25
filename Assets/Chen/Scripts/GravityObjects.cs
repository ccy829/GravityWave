using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObjects : MonoBehaviour {
    public float gravityScale;
    float gravity;
    Rigidbody2D r2d;


    Quaternion downRotation;
    Quaternion upRotation;

    float rotationSmoothing;
    float rotationLerp;
    float rotationLerpSpeed = 10f;
    public bool canRotate = true;
    int gravityDir;
    // Use this for initialization
    void Awake()
    {
        r2d = GetComponent<Rigidbody2D>();

        downRotation = Quaternion.Euler(0, 0, 0);
        upRotation = Quaternion.Euler(180, 0, 0);
    }
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gravity = GameManager.GetInstance().GetGravity();
        r2d.gravityScale = gravity *gravityScale;

        if (gravity > 0)
        {
            gravityDir = 1;
        }
        else
        {
            gravityDir = -1;
        }
        if (canRotate)
        {
            rotationLerp += gravityDir * rotationLerpSpeed * Time.deltaTime;
            if (Mathf.Abs(rotationLerp) > 1)
            {
                rotationLerp = gravityDir;
            }

            transform.rotation = Quaternion.Lerp(downRotation, upRotation, rotationLerp);
        }
        
    }
}
