using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGun : MonoBehaviour {
    public GameObject gun;
    public GameObject deadGodModel;
    public GameObject deadGun;
    public GameObject shieldModel;
    public GameObject kulou;
    public GameObject shieldPartical;
    bool added = false;
    public bool random;
    public enum buffTypes {
        superGun=0,
        deadGod,
        shield
    }
    public buffTypes buffType;
    public float superGunBuffTime=10;
    public float deadGodBuffTime = 15;
	// Use this for initialization
	void Start () {
		
	}
    void Awake()
    {
        if (random)
        {
            buffType= (buffTypes)(int)Random.Range(0,2.99f);
            
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {         
            if (!added)
            {
                added = true;

                switch (buffType)
                {
                    case (buffTypes.deadGod):
                        other.GetComponent<PlayerController>().TransToDeadGod(deadGodBuffTime);
                        other.GetComponent<PlayerController>().SpawnGun(deadGun);
                        other.GetComponent<PlayerController>().SpawnModel(deadGodModel);
                        other.GetComponent<PlayerController>().SpawnKulou(kulou);
                        break;
                    case (buffTypes.superGun):
                        other.GetComponent<PlayerController>().TransToSuperGun(superGunBuffTime);
                        other.GetComponent<PlayerController>().SpawnGun(gun);
                        break;
                    case (buffTypes.shield):
                        other.GetComponent<PlayerController>().SpawnShield(shieldModel);
                        other.GetComponent<PlayerController>().ShieldPartical = shieldPartical;
                        break;
                }              
                Destroy(gameObject);

            }
            
        }
    }
}
