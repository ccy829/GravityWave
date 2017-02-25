using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public static PlayerInput _instance;
    PlayerController[] players;
    string[] playerString;
    Vector2[] axisInput;

    public static PlayerInput GetInstance()
    {
        return _instance;
    }
    void Awake()
    {
        _instance = this;
        players = new PlayerController[4];

        playerString = new string[4];
        playerString[0] = "P1";
        playerString[1] = "P2";
        playerString[2] = "P3";
        playerString[3] = "P4";

        axisInput = new Vector2[4];
    }
    void Update()
    {
        UpdateAxis();
        //Debug.Log(axisInput[0].ToString());
    }
    void UpdateAxis()
    {
        for (int i = 0; i < 4; i++)
        {
            axisInput[i].x = Input.GetAxis(playerString[i]+ "Horizontal");
            axisInput[i].y = Input.GetAxis(playerString[i] + "Vertical");
        }
    }



    public Vector2 GetInputAxis(int playerIndex)
    {     
        
        return axisInput[playerIndex];
    }

    public bool GetJumpDown(int playerIndex)
    {
        
        return Input.GetButtonDown(playerString[playerIndex] + "Jump");
    }

    public bool GetFireDown(int playerIndex)
    {
        return Input.GetButtonDown(playerString[playerIndex] + "Fire");
    }
    public bool GetFire(int playerIndex)
    {
        return Input.GetButton(playerString[playerIndex] + "Fire");
    }
    public bool GetFire2Down(int playerIndex)
    {
        return Input.GetButtonDown(playerString[playerIndex] + "Fire2");
    }

}
