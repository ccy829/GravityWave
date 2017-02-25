using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public int index;
    [HideInInspector]
    public GameObject model;
    public GameObject ShieldPartical;
    Vector2 inputAxis;
    bool inputJump;
    bool inputFireDown;
    bool inputFire;
    bool inputFire2Down;


    PlayerInput playerInput;

    Rigidbody2D rigidbody;
    [HideInInspector]
    public Gun gun;

    public float speed;
    public float acc;
    public float accBack;
    public float jumpSpeed;
    public float airForce;
    float maxYSpeed = 9f;
    public LayerMask groundMask;

    bool grounded;

    public float gravityScale;
    float gravity;

    GameObject kulou;

    public float skinWidth;
    Collider2D colliderr;
    Vector2[] raycastOrigins;
    int gravityDir = -1;
    int movingDirection;
    public int facingDirection=1;
    Quaternion downRotation;
    Quaternion upRotation;

    float rotationSmoothing;
    float rotationLerp;
    float rotationLerpSpeed = 10f;
    [HideInInspector]
    public bool alive = true;
    [HideInInspector]
    public Animator animator;
    public Transform gunPosition;
    public GameObject defultGun;
    public Transform modelPosition;
    public GameObject defultModel;
    public GameObject addGunPartical;

    bool supergun=false;

    bool shield = false;
    float buffTime = 10f;
    float buffTimer = 0f;
    bool buffing = false;
    bool deadGod=false;
    float deadGodTime;

    float normalGravityScale;
    GameObject shieldModel;
    void UpdateRaycastOrigins()
    {
        Bounds bounds = colliderr.bounds;
        bounds.Expand(skinWidth * -2);

        if (gravityDir == -1)
        {
            raycastOrigins[0] = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins[1] = new Vector2((bounds.min.x + bounds.max.x) * 0.5f, bounds.min.y);
            raycastOrigins[2] = new Vector2(bounds.max.x, bounds.min.y);
        }
        else
        {
            raycastOrigins[0] = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins[1] = new Vector2((bounds.min.x + bounds.max.x) * 0.5f, bounds.max.y);
            raycastOrigins[2] = new Vector2(bounds.max.x, bounds.max.y);
        }
        
    }
    // Use this for initialization
    void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
        colliderr = GetComponent<Collider2D>();
        raycastOrigins = new Vector2[3];
        
        downRotation = Quaternion.Euler(0,0,0);
        upRotation = Quaternion.Euler(0,0,180);
        normalGravityScale = gravityScale;
        SpawnGun(defultGun);
        SpawnModel(defultModel);
    }
	void Start () {
        playerInput = PlayerInput.GetInstance();
        if (index < GameManager.GetInstance().playerCount)
        {
            GameManager.GetInstance().players[index] = this;
            GameManager.GetInstance().camera2d.AddCameraTarget(this.transform);
        }
        else
        {
            Destroy(GameManager.GetInstance().playerFollower[index].gameObject);
            Destroy(gameObject);
        }
    }
    void CheckYVelocity()
    {
        if (Mathf.Abs(rigidbody.velocity.y) > maxYSpeed)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, maxYSpeed*rigidbody.velocity.y/ Mathf.Abs(rigidbody.velocity.y));
        }
    }
	// Update is called once per frame
	void Update () {
        UpdateInput();
        UpdateRaycastOrigins();
        UpdateGravity();
        CheckGround();
	    CheckFire();
        updateBuff();
        //如果不是同向且超速
        if (inputAxis.x != 0)
        {
            facingDirection = (int)(inputAxis.x / Mathf.Abs(inputAxis.x));
            if (!(rigidbody.velocity.x * inputAxis.x > 0 && Mathf.Abs(rigidbody.velocity.x) > speed))
            {
                rigidbody.velocity += Vector2.right * acc * inputAxis.x * Time.deltaTime;
                if (rigidbody.velocity.x * inputAxis.x > 0 && Mathf.Abs(rigidbody.velocity.x) > speed)
                {
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x / Mathf.Abs(rigidbody.velocity.x) * speed, rigidbody.velocity.y);
                }
            }
        }
            
        if (inputAxis.x == 0)
        {
           
            float reduceTemp = accBack * Time.deltaTime;
            if (Mathf.Abs(rigidbody.velocity.x) > reduceTemp)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x - rigidbody.velocity.x / Mathf.Abs(rigidbody.velocity.x) * reduceTemp, rigidbody.velocity.y);
            }
            else
            {
                rigidbody.velocity = new Vector2(0,rigidbody.velocity.y);
            }
           
        }
        if (inputAxis.y != 0)
        {
            if (!((inputAxis.y * rigidbody.velocity.y > 0) && (Mathf.Abs(rigidbody.velocity.y) >= maxYSpeed)))
            {
                rigidbody.AddForce(Vector2.up * airForce * inputAxis.y);
            }
            
        }
        if (inputJump&&grounded)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x,jumpSpeed*-gravityDir);
        }

        modelPosition.transform.localRotation = Quaternion.Euler(0, 90 - 45 * facingDirection * -gravityDir, 0);
        gun.transform.localRotation = Quaternion.Euler(0, 90 - 90 * facingDirection * -gravityDir, 0);
        UpdateAnmation();
    }

    void CheckFire()
    {
        if (inputFireDown&&!deadGod)
        {
            //子弹
            gun.Fire(1);
        }
        if (inputFire2Down&&!supergun)
        {
            //Debug.Log("Fire2");
            gun.Fire(0);
        }
        if (inputFire &&!deadGod)
        {
            gun.FireAuto();
        }
    }

    void CheckGround()
    {
        float rayLength =skinWidth * 2;
        for (int i = 0; i < 3; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigins[i], Vector2.up*gravityDir, rayLength, groundMask);
            Debug.DrawLine(raycastOrigins[i], raycastOrigins[i]+Vector2.up * gravityDir* skinWidth * 2);
            if (hit)
            {
                grounded = true;
                return;
            }
        }
        grounded = false;

    }
    void UpdateInput()
    {
        inputAxis = playerInput.GetInputAxis(index);
        inputFire = playerInput.GetFire(index);
        inputFireDown = playerInput.GetFireDown(index);
        inputFire2Down= playerInput.GetFire2Down(index);
        inputJump = playerInput.GetJumpDown(index);
        //Debug.Log(inputAxis.ToString()); 
    }
    void UpdateAnmation()
    {
        if (rigidbody.velocity.x != 0 && grounded)
        {
            animator.SetBool("Run",true);
            
            return;
        }
        animator.SetBool("Run", false);
       
    }
    void UpdateGravity()
    {
        gravity = GameManager.GetInstance().GetGravity();
        rigidbody.gravityScale = gravity * gravityScale;

        if (gravity > 0)
        {
            gravityDir = 1;
        }
        else
        {
            gravityDir = -1;
        }

        rotationLerp += gravityDir * rotationLerpSpeed * Time.deltaTime;
        if (Mathf.Abs(rotationLerp) > 1)
        {
            rotationLerp = gravityDir;
        }

        transform.rotation = Quaternion.Lerp(downRotation,upRotation,rotationLerp);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hurt")
        {
            Dead();
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Hurt")
        {
            Dead();
        }
    }
    public void Dead()
    {
        if (alive&&!GameManager.GetInstance().over)
        {
            if (shield)
            {
                Destroy(shieldModel);
                shield = false;
                Instantiate(ShieldPartical,modelPosition.position,transform.rotation);
            }
            else
            {
                if (kulou != null)
                {
                    Destroy(kulou.gameObject);
                }
                alive = false;
                GameManager.GetInstance().camera2d.RemoveCameraTarget(this.transform);
                ParticalCreater.GetInstance().SpawnDeadPartical(transform.position);
                GameManager.GetInstance().Kill(index);
            }
            
        }
    }
    public void SpawnGun(GameObject gunPfb)
    {
        if (gun != null)
        {
            Destroy(gun.gameObject);
        }
        gun=Instantiate(gunPfb,gunPosition.position,gunPosition.rotation).GetComponent<Gun>();
        gun.transform.SetParent(gunPosition);
        gun.player = this;
        Instantiate(addGunPartical,gunPosition.position,transform.rotation);
    }
    public void TransToSuperGun(float _buffTime)
    {
        EndBuff();
        supergun = true;
        buffTime = _buffTime;
        buffTimer = 0;
        buffing = true;
    }
    public void TransToDeadGod(float _buffTime)
    {
        EndBuff();
        deadGod = true;
        buffTime = _buffTime;
        buffTimer = 0;
        buffing = true;
    }
    void EndBuff()
    {
        supergun = false;
        deadGod = false;
        SpawnGun(defultGun);
        SpawnModel(defultModel);
        gravityScale = normalGravityScale;
        buffing = false;

        if (kulou != null)
        {
            Destroy(kulou.gameObject);
        }
    }
    void updateBuff()
    {
        if (buffing)
        {
            buffTimer += Time.deltaTime;
            if (buffTimer >= buffTime)
            {
                EndBuff();
            }
        }
    }
    public void SpawnModel(GameObject _model)
    {
        if (model != null)
        {
            Destroy(model.gameObject);
        }
        GameObject modelIns = Instantiate(_model, modelPosition.position, modelPosition.rotation);
        modelIns.transform.parent = modelPosition;
        model = modelIns.gameObject;
        animator = modelIns.GetComponent<ModelInfo>().animator;
    }
    public void SpawnShield(GameObject _shieldModel)
    {
        if (shieldModel == null)
        {
            shieldModel = Instantiate(_shieldModel, modelPosition.position, modelPosition.rotation);
            shieldModel.transform.parent = modelPosition;
        }
        shield = true;
    }
    public void SpawnKulou(GameObject _kulou)
    {
        if (kulou != null)
        {
            Destroy(kulou.gameObject);
        }
        kulou = Instantiate(_kulou, modelPosition.position, Quaternion.Euler(0, 0, 0));
        kulou.GetComponent<Follower>().target = transform;

        
    }
    
}
