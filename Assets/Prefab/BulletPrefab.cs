using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPrefab : MonoBehaviour {

    private Rigidbody2D thisRigid;
    public GameObject Partical1;
    public GameObject Partical2;

    public float gravityScale;
    public Transform modelTransform;
    float gravity;

    void Awake()
    {
        thisRigid = this.transform.GetComponent<Rigidbody2D>();
        //Instantiate(Partical1, transform.position, transform.rotation).transform.SetParent();
        //StartCoroutine(Partical());
    }
    void Update()
    {
        transform.position = (Vector2)transform.position;
        UpdateGravity();
        CalculateRotation();
    }

    void UpdateGravity()
    {
        gravity = GameManager.GetInstance().GetGravity();
        thisRigid.gravityScale = gravity * gravityScale;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject.Instantiate(Partical2).transform.position = this.transform.position;
        GameObject.Destroy(this.gameObject);
    }
    /*IEnumerator Partical()
    {
        yield return null;
        GameObject.Instantiate(Partical1).transform.position = this.transform.position;
    }*/

    void CalculateRotation()
    {
        modelTransform.LookAt(transform.position+(Vector3)thisRigid.velocity);
    }
}
