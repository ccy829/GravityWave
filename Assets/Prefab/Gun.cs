using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
public class Gun : MonoBehaviour
{
    
    public GameObject Boom;//子弹
    public GameObject Bullet2;//子弹
    public PlayerController player;
    public Transform Muzzle;//枪口
    public float BombIntensity = 400f;//炸弹发射力度
    public float BulletSpeed = 2000f;//子弹发射力度
    public float AttackSpeed = 1000f;//近身攻击发射力度
    public float boomTime = 1;//炸弹间隔.
    private float timer1;
    public float fireTime = 1;//子弹间隔.
    private float timer2;
    int forceY = 0;
    public bool auto = false;
    public GameObject Partical1;
    public GameObject readyPartical;
    public Animator animator;

    bool cooling = false;
    public void FireAuto()
    {
        if (auto)
        {
            Fire(1);
        }
    }
    public void Fire(int Index)
    {

        if (Index == 0)// 扩散炸弹
        {
            if (timer1 < boomTime)
            {
                return;
            }
            animator.SetTrigger("Shoot");
            //ProCamera2DShake.Instance.Shake();
            timer1 = 0;
            cooling = true;
            GameObject go = GameObject.Instantiate(Boom,player.transform.position,player.transform.rotation);
            go.GetComponent<BombPrefab>().player = player.gameObject;
            /*
            go.GetComponent<BombPrefab>().thisType = BombPrefab.BombType.Bomb;
            go.transform.position = new Vector3(Muzzle.position.x, Muzzle.position.y,0);
            Rigidbody2D r2d = go.GetComponent<Rigidbody2D>();
            if (GameManager.GetInstance().GetGravity() > 0)
            {
                forceY = -1;
            }
            else if (GameManager.GetInstance().GetGravity() < 0)
            {
                forceY = 1;
            }
            if (Muzzle.position.x > this.transform.position.x)
            {
                r2d.AddForce(new Vector2(1, forceY) * BombIntensity);
            }
            else
            {
                r2d.AddForce(new Vector2(-1, forceY) * BombIntensity);
            }
            */
        }
        else if (Index == 1)// 子弹攻击
        {
            if (timer2 < fireTime)
            {
                return;
            }
            GameObject fireP=Instantiate(Partical1, Muzzle.position, Muzzle.rotation);
            //fireP.transform.localScale = Vector3.one;
            fireP.transform.SetParent(this.transform);
            
            animator.SetTrigger("Shoot");
            timer2 = 0;
            GameObject go = GameObject.Instantiate(Bullet2);
            go.transform.position = new Vector3(Muzzle.position.x, Muzzle.position.y, 0);
            Rigidbody2D r2d = go.GetComponent<Rigidbody2D>();
            if (Muzzle.position.x > this.transform.position.x)
            {
                r2d.AddForce(new Vector2(1, 0) * BulletSpeed);
            }
            else
            {
                r2d.AddForce(new Vector2(-1, 0) * BulletSpeed);
            }
        }
        else if (Index == 2)// 水平击飞
        {
            //定义爆炸位置
            Vector3 explosionPos = Muzzle.position;
            float radius = 3f;
            //把爆炸范围内的物体收集起来
            Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);//.OverlapSphere(explosionPos, radius);//第一个参数是位置,第二参数是范围半径

            foreach (Collider2D hit in colliders)
            {
                Rigidbody2D r2d = hit.gameObject.GetComponent<Rigidbody2D>();
                if (r2d != null)
                {
                    if (player.facingDirection == 1)
                    {
                        if (r2d.transform.position.x > this.transform.position.x)
                        {
                            r2d.AddForce(new Vector2(1, 0) * AttackSpeed);
                        }
                    }
                    else
                    {
                        if (r2d.transform.position.x < this.transform.position.x)
                        {
                            r2d.AddForce(new Vector2(-1, 0) * AttackSpeed);
                        }
                    }
                }
            }
        }
        else if (Index == 3)// 引力炸弹
        {
            if (timer1 < boomTime)
            {
                return;
            }
            timer1 = 0;
            GameObject go = GameObject.Instantiate(Boom);
            go.transform.position = new Vector3(Muzzle.position.x, Muzzle.position.y, 0);
            Rigidbody2D r2d = go.GetComponent<Rigidbody2D>();
            if (GameManager.GetInstance().GetGravity() > 0)
            {
                forceY = -1;
            }
            else if (GameManager.GetInstance().GetGravity() < 0)
            {
                forceY = 1;
            }
            if (Muzzle.position.x > this.transform.position.x)
            {
                r2d.AddForce(new Vector2(1, forceY) * BombIntensity);
            }
            else
            {
                r2d.AddForce(new Vector2(-1, forceY) * BombIntensity);
            }
        }
    }

    void Update()
    {
        if (timer1 < boomTime)//炸弹
        {
            timer1 += Time.deltaTime;
        }
        if (cooling)
        {
            if (timer1 >= boomTime)
            {
                //cool
                GameObject ins= Instantiate(readyPartical,player.transform.position,transform.rotation);
                ins.transform.SetParent(player.transform);
                cooling = false;
            }
        }
        if (timer2 < fireTime)
        {
            timer2 += Time.deltaTime;
        }
    }
    void Awake()
    {
       // animator = this.GetComponent<Animator>();
    }

}
