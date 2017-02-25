using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPrefab : MonoBehaviour
{ 
    public enum BombType
    {
        Bomb,
        Gravitation
    }
    
    private Rigidbody2D thisRigid;
    public GameObject Partical1;
    public GameObject Partical2;
    public BombType thisType;
    public float radius = 3f;
    public float gravityScale;
    float gravity;
    public float force = 100;//爆炸力
    public GameObject player;
    void Awake()
    {
        thisRigid = this.transform.GetComponent<Rigidbody2D>();
        StartCoroutine(Partical());
    }

    void Update()
    {
        UpdateGravity();
    }

    void UpdateGravity()
    {
        gravity = GameManager.GetInstance().GetGravity();
        thisRigid.gravityScale = gravity * gravityScale;
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        //爆炸半径
        

        //定义爆炸位置
        Vector3 explosionPos = transform.position;

        //把爆炸范围内的物体收集起来
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);//.OverlapSphere(explosionPos, radius);//第一个参数是位置,第二参数是范围半径

        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D r2d = hit.gameObject.GetComponent<Rigidbody2D>();
            if (r2d != null&&hit.gameObject!=player)
            {
                if (thisType == BombType.Bomb)
                {
                    Vector2 v2 = hit.transform.position - transform.position;
                    v2 = v2 * radius - v2;
                    r2d.AddForce(v2.normalized * force);
                }
                else if (thisType == BombType.Gravitation)
                {
                    Vector2 v2 =  transform.position - hit.transform.position;
                    //v2 = v2 - v2.normalized * radius;
                    r2d.AddForce(v2.normalized * force);
                }
            }
        }
        GameObject.Instantiate(Partical2).transform.position = this.transform.position;
        GameObject.Destroy(this.gameObject);
    }

    IEnumerator Partical()
    {
        yield return null;
        GameObject.Instantiate(Partical1).transform.position = this.transform.position;
    }
}
