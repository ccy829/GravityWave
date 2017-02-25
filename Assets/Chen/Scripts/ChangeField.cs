using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeField : MonoBehaviour {
    public float time;
    float timer;
    //public float delay;
    //float delayTimer;
    bool started = false;
    public Transform min;
    public Transform max;
    public List<Transform> needToChange;

    public spawnItems[] spawnItem;

	// Use this for initialization
	void Start () {

        ChangeFields();
    }
	
	// Update is called once per frame
	void Update () {

            timer += Time.deltaTime;
            if (timer >= time)
            {
                timer = 0;
                ChangeFields();
            }

        UpdateItems();

    }
    Vector2 getRandomPoint()
    {
        Vector2 v2;
        v2.x = Random.Range(min.position.x,max.position.x);
        v2.y = Random.Range(min.position.y,max.position.y);
        return v2;
    }

    void ChangeFields()
    {
        if (needToChange.Count != 0)
        {
            for (int i = 0; i < needToChange.Count; i++)
            {
                needToChange[i].transform.position = getRandomPoint();
            }
        }
        
    }
    void UpdateItems()
    {
        for (int i = 0; i < spawnItem.Length; i++)
        {
            if (!spawnItem[i].itemSpawned)
            {
                spawnItem[i].timer += Time.deltaTime;
                if (spawnItem[i].timer > spawnItem[i].time)
                {
                    spawnItem[i].itemSpawned = true;
                    GameObject ins=Instantiate(spawnItem[i].item,getRandomPoint(),Quaternion.Euler(0,0,0));
                    if (spawnItem[i].addToField)
                    {
                        needToChange.Add(ins.transform);
                    }
                    
                }
            }
            
        }
    }
}

[System.Serializable]
public class spawnItems
{
    public GameObject item;
    public float time;
    [HideInInspector]
    public bool itemSpawned=false;
    [HideInInspector]
    public float timer;

    public bool addToField = false;

    
}
