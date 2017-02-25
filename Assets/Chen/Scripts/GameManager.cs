using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.Audio;
public class GameManager : MonoBehaviour {
    public enum gameModes {
        single,
        team
    }
    public int playerCount=4;
    public gameModes gameMode;
    public GameObject[] playerFollower;
    [HideInInspector]
    public PlayerController[] players;
    bool[] alive;
    int aliveCount = 4;
    public static GameManager _instance;
    [Range(-1,1)]
    public float gravity;
    public ProCamera2D camera2d;

    float slowTime = 0.55f;
    float slowTimer = 0;
    bool slowTiming = false;
    public bool over = false;
    public AudioMixerSnapshot snapNormal;
    public AudioMixerSnapshot snapLowpass;
    void Awake() {
        NormalTime();
        playerCount=UI_start.PlayerNumbers;
        _instance = this;
        players = new PlayerController[playerCount];
        aliveCount = playerCount;
        alive = new bool [4] { true, true, true, true };
        
    }

    public static GameManager GetInstance()
    {
        return _instance;
    }
    public float GetGravity()
    {
        return gravity;
    }
    void Start () {
        //normalPass();
	}
	
	// Update is called once per frame
	void Update () {
        gravity = GravityCurve.getValue();
        CheckSlowTime();

    }
    public void Kill(int _index)
    {
        aliveCount -= 1;
        alive[_index] = false;
        playerFollower[_index].SetActive(false);
        players[_index].gameObject.SetActive(false);
        ProCamera2DShake.Instance.Shake();
        
        CheckWin();
    }
    void WinSingle(int _index)
    {
        over = true;
        SlowTime();
        //LowPass();
        
        MySceneManager.GetInstance().RoundOver(_index);
    }
    void SlowTime()
    {
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = 0.004f;
        slowTiming = true;
        LowPass();
    }
    void NormalTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        slowTiming = false;
        normalPass();
    }
    void CheckSlowTime()
    {
        if (slowTiming)
        {
            slowTimer += Time.deltaTime;
            if (slowTimer > slowTime)
            {
                slowTiming = false;
                NormalTime();
            }
        }
        
    }
    void LowPass()
    {
        snapLowpass.TransitionTo(0.1f);
    }
    void normalPass()
    {
        snapNormal.TransitionTo(2);
    }
    void CheckWin()
    {
        if (gameMode == gameModes.single)
        {
            if (aliveCount <= 1)
            {
                //Debug.Log("Who Wins");
                for (int i = 0; i < playerCount; i++)
                {
                    if (alive[i])
                    {
                        WinSingle(i);
                        return;
                    }
                }
            }
        }
        if (gameMode == gameModes.team)
        {
            if (aliveCount <= 2)
            {

            }
        }
    }
}
