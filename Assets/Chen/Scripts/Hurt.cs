using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurt : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        checkPlayer();

    }
    void checkPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.21f);//.OverlapSphere(explosionPos, radius);//第一个参数是位置,第二参数是范围半径

        foreach (Collider2D hit in colliders)
        {
            if (hit.tag=="Player")
            {
                hit.gameObject.GetComponent<PlayerController>().Dead();
            }
        }
    }
}
