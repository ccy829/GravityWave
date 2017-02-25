using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalDestroy : MonoBehaviour
{

    public float Timer;//销毁时间
    private float Thistimer;
	void Update ()
	{
	    Thistimer += Time.deltaTime;
	    if (Thistimer > Timer)
	    {
	        GameObject.Destroy(this.gameObject);
	    }

	}
}
