using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalCreater : MonoBehaviour {
    public static ParticalCreater _instance;

    public GameObject deadPartical;

    public static ParticalCreater GetInstance()
    {
        return _instance;
    }
	// Use this for initialization
	void Awake () {
        _instance = this;
	}

    public void SpawnDeadPartical(Vector3 _position)
    {
        Instantiate(deadPartical,_position,transform.rotation);
    }
}
