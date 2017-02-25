using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class UIprefabManagement {
    public static bool instantiated = false;
}
public class UI_start : MonoBehaviour {
    public GameObject UIprefab;
    public static bool GameStart;
    public static int PlayerNumbers {
        get {
            return _PlayerNumbers;
        }
    }
    GameObject UI;
    bool select = false;
    static int _PlayerNumbers;
    static Vector3 psPos;
    static MySceneManager MSM;
    static TweenAlpha title;
    static TweenAlpha ins;
    static TweenPosition up;
    static TweenPosition down;
    static TweenPosition twPos_ps;
    static Text playerSetting;


    void Awake() {
        if (!UIprefabManagement.instantiated) {
            UI = Instantiate(UIprefab);
            MSM = UI.GetComponent<MySceneManager>();
            title = UI.GetComponent<UIObjectIndex>().UIStart_Title;
            ins = UI.GetComponent<UIObjectIndex>().UIStart_Ins;
            up = UI.GetComponent<UIObjectIndex>().UIStart_ArrowUp;
            down = UI.GetComponent<UIObjectIndex>().UIStart_ArrowDown;
            playerSetting = UI.GetComponent<UIObjectIndex>().UIStart_PlayerSetting;
            twPos_ps = playerSetting.GetComponent<TweenPosition>();
            UIprefabManagement.instantiated = true;
        }
        MySceneManager.OnLoadScene += hideTitle;
    }

	// Use this for initialization
	void Start () {
        _PlayerNumbers = 2;
        psPos = playerSetting.rectTransform.localPosition;
        hideGravity();
        MSM.showScoreboard(false);
        GameStart = false;
        title.Play(false);
        ins.Play(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return) && !GameStart) {
            if (!select)
            {
                select = true;
                ins.Stop();
                up.GetComponent<Image>().enabled = true;
                playerSetting.GetComponent<Text>().enabled = true;
                down.GetComponent<Image>().enabled = true;
                up.Play(false);
                down.Play(false);
                UpdatePlayer();
            }
            else
            {
                GameStart = true;
                up.Stop();
                down.Stop();
                up.GetComponent<Image>().enabled = false;
                playerSetting.GetComponent<Text>().enabled = false;
                down.GetComponent<Image>().enabled = false;
                loadLevel();
            }
        }
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && select)
        {
            if (!GameStart) {
                twPos_ps.TargetPosition = psPos - new Vector3(0, 16.0f, 0);
                twPos_ps.Play(true);
                _PlayerNumbers -= (_PlayerNumbers > 2) ? 1 : (_PlayerNumbers - 4);
                UpdatePlayer();
            }
        }
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && select)
        {
            if (!GameStart) {
                twPos_ps.TargetPosition = psPos + new Vector3(0, 16.0f, 0);
                twPos_ps.Play(true);
                _PlayerNumbers += (_PlayerNumbers < 4) ? 1 : (2 - _PlayerNumbers);
                UpdatePlayer();
            }
        }
    }

    void UpdatePlayer() {
        playerSetting.text = _PlayerNumbers.ToString() + " PLAYERS";
    }

    void hideGravity() {
        MSM.gravity.hideCurve();
        MSM.panelGravity.hideMonitor();
    }

    void hideTitle() {
        title.Stop();
    }

    void loadLevel() {
        StartCoroutine(coroutine_LoadLevel());
    }

    IEnumerator coroutine_LoadLevel() {
        MSM.fadeIn();
        yield return new WaitForSeconds(MSM.fadeSpeed);
        GravityCurve.Reset();
        MSM.gravity.showCurve();
        MSM.panelGravity.showMonitor();
        MSM.showScoreboard(true);
        MSM.InitializedScoreboard(_PlayerNumbers);
        MySceneManager.LoadScene(MSM.scenes[0]);
    }
}
