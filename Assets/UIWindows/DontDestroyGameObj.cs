using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyGameObj : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
    }
}
