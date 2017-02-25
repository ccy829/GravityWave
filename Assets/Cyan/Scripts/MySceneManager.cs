using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour {
    public delegate void SceneEventHandler();
    private static SceneEventHandler SceneEvent;
    public delegate void GameEventHandler();
    private static GameEventHandler GameEvent;

    public static MySceneManager _instance;
    public static event SceneEventHandler OnLoadScene
    {
        add
        {
            SceneEvent += value;
        }

        remove
        {
            SceneEvent -= value;
        }
    }
    public static event GameEventHandler OnGameOver
    {
        add
        {
            GameEvent += value;
        }

        remove
        {
            GameEvent -= value;
        }
    }
    public GameObject prefabScore;
    public GravityCurve gravity;
    public LineZero panelGravity;
    public Image panelFade;
    public Image panelScore;
    public Image panelRoundOver;
    public Image panelGameOver;
    public Text roundWinner;
    public Text WinnerScore;
    public Text[] scoreboard;
    public Text[] result_name;
    public Text[] result_score;
    public TweenAlpha twAlp_winner, twAlp_result, twAlp_textscore, twAlp_score, twAlp_result_ins;
    public TweenAlpha[] twAlp_result_name;
    public TweenAlpha[] twAlp_result_score;
    public TweenScale twScl_score;
    public float fadeSpeed = 0.15f;
    public AnimationCurve Fade;
    public string[] playerName;
    public int[] scenes;
    int currentLevel = 0;
    float currentTime;
    bool isFadein = false;
    bool isPlaying = false;

    static int[] score;
    static int currentWinner = 0;
    static bool isRoundEnd = false;
    static bool isOver = false;
    static bool initializedscoreboard = false;
    static ScoreboardElement[] board;
    IEnumerator coroutine_Fade;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        score = new int[playerName.Length];
        for (int i = 0; i < score.Length; i++)
        {
            score[i] = 0;
        }
        SceneEvent += fadeOut;
        _instance = this;
    }
    public static MySceneManager GetInstance()
    {
        return _instance;
    }
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Return) && isOver)
        {
            ToMain();
        }
	}

    void NextLevel() {
        StartCoroutine(coroutine_Next());
    }

    void ToMain() {
        UI_start.GameStart = false;
        isRoundEnd = false;
        isOver = false;
        currentLevel = 0;
        for (int i = 0; i < score.Length; i++)
        {
            score[i] = 0;
        }
        StartCoroutine(coroutine_ToMain());
    }

    public void fadeIn()
    {
        if (isPlaying) StopCoroutine(coroutine_Fade);
        isFadein = true;
        coroutine_Fade = animFade();
        StartCoroutine(coroutine_Fade);
    }

    public void fadeOut()
    {
        if (isPlaying) StopCoroutine(coroutine_Fade);
        isFadein = false;
        coroutine_Fade = animFade();
        StartCoroutine(coroutine_Fade);
    }

    public void RoundOver(int winner)
    {
        if (!isOver) {
            isRoundEnd = true;
            currentWinner = winner;
            
            roundWinner.text = playerName[winner] + " survived";
            score[winner]++;
            updateScoreboard();
            showRoundOverPanel();
            Invoke("NextLevel", 1.75f);
        }
    }

    public static void LoadScene(int id) {
        SceneManager.LoadScene(id);
        if (SceneEvent != null) SceneEvent();
    }

    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
        if (SceneEvent != null) SceneEvent();
    }

    public void InitializedScoreboard(int numplayer) {
        board = new ScoreboardElement[numplayer];
        panelScore.rectTransform.sizeDelta = new Vector2(84.0f * numplayer, 96.0f);
        for (int i = 0; i < board.Length; i++) {
            var go = Instantiate(prefabScore).GetComponent<RectTransform>();
            go.transform.SetParent(panelScore.transform);
            go.localPosition = Vector3.zero;
            go.localScale = Vector3.one;
            go.anchoredPosition = new Vector2(50.0f + 82.0f * i, 0);
            board[i] = go.GetComponent<ScoreboardElement>();
            board[i].Name.text = (i + 1).ToString() + "P";
            board[i].score.text = "0";
        }
        initializedscoreboard = true;
    }

    public void showScoreboard(bool toggle)
    {
        panelScore.gameObject.SetActive(toggle);
    }

    public void updateScoreboard()
    {
        if (initializedscoreboard) {
            for (int i = 0; i < board.Length; i++)
            {
                board[i].score.text = score[i].ToString();
            }
        }
    }

    void showRoundOverPanel() {
        twAlp_winner.Play(false);
        twAlp_textscore.Play(false);
        twAlp_score.Play(false);
        StartCoroutine(animRoundPanel());
    }

    void HideRoundOverPanel() {
        twAlp_winner.Stop();
        twAlp_textscore.Stop();
        twAlp_score.Stop();
        twScl_score.Stop();
        panelRoundOver.color = new Color(panelRoundOver.color.r, panelRoundOver.color.g, panelRoundOver.color.b, 0);
        panelRoundOver.rectTransform.sizeDelta = new Vector2(16, 0);
    }

    void showGameOverPanel() {
        twAlp_result.Play(false);
        for (int i = 0; i < UI_start.PlayerNumbers; i++) {
            result_name[i].text = playerName[i];
            result_score[i].text = score[i].ToString();
            twAlp_result_name[i].Play(false);
            twAlp_result_score[i].Play(false);
        }
        twAlp_result_ins.Play(false);
        StartCoroutine(animGameOverPanel());
    }

    void hideGameOverPanel() {
        twAlp_result.Stop();
        for (int i = 0; i < twAlp_result_name.Length; i++)
        {
            twAlp_result_name[i].Stop();
            twAlp_result_score[i].Stop();
        }
        twAlp_result_ins.Stop();
        panelGameOver.color = new Color(panelGameOver.color.r, panelGameOver.color.g, panelGameOver.color.b, 0);
        panelGameOver.rectTransform.sizeDelta = new Vector2(16, 0);
    }

    IEnumerator animGameOverPanel()
    {
        float speed = 0.15f;
        float cur = 0;

        while (cur < speed)
        {
            cur += (cur + Time.deltaTime <= speed) ? Time.deltaTime : speed - cur;
            panelGameOver.color = new Color(panelGameOver.color.r, panelGameOver.color.g, panelGameOver.color.b, 0.8f * (cur / speed));
            panelGameOver.rectTransform.sizeDelta = new Vector2(16, 440.0f * (cur / speed));
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator animRoundPanel() {
        float speed = 0.1f;
        float cur = 0;

        WinnerScore.text = (score[currentWinner] - 1).ToString();
        while (cur < speed) {
            cur += (cur + Time.deltaTime <= speed) ? Time.deltaTime : speed - cur;
            panelRoundOver.color = new Color(panelRoundOver.color.r, panelRoundOver.color.g, panelRoundOver.color.b, 0.8f * (cur / speed));
            panelRoundOver.rectTransform.sizeDelta = new Vector2(16, 144.0f * (cur / speed));
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(0.5f);
        WinnerScore.text = score[currentWinner].ToString();
        twScl_score.Play(true);
    }

    IEnumerator coroutine_Next()
    {
        if (currentLevel < scenes.Length - 1)
        {
            fadeIn();
            yield return new WaitForSeconds(fadeSpeed);
            currentLevel++;
            HideRoundOverPanel();
            isRoundEnd = false;
            GravityCurve.Reset();
            LoadScene(scenes[currentLevel]);
        }
        else
        {
            isOver = true;
            HideRoundOverPanel();
            gravity.hideCurve();
            panelGravity.hideMonitor();
            showGameOverPanel();
            showScoreboard(false);
            if (GameEvent != null) GameEvent();
        }
    }

    IEnumerator coroutine_ToMain() {
        fadeIn();
        yield return new WaitForSeconds(fadeSpeed);
        hideGameOverPanel();
        for (int i = 0; i < board.Length; i++) {
            Destroy(board[i].gameObject);
        }
        initializedscoreboard = false;
        LoadScene("UI_start");
    }

    IEnumerator animFade() {
        isPlaying = true;
        if (isFadein)
        {
            currentTime = 0;
            while (currentTime < fadeSpeed)
            {
                currentTime += (currentTime + Time.deltaTime <= fadeSpeed) ? Time.deltaTime : fadeSpeed - currentTime;
                panelFade.color = new Color(panelFade.color.r, panelFade.color.g, panelFade.color.b, Fade.Evaluate(currentTime / fadeSpeed));
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        else
        {
            currentTime = fadeSpeed;
            while (currentTime > 0)
            {
                currentTime -= (currentTime - Time.deltaTime >= 0) ? Time.deltaTime : currentTime;
                panelFade.color = new Color(panelFade.color.r, panelFade.color.g, panelFade.color.b, Fade.Evaluate(currentTime / fadeSpeed));
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        isPlaying = false;
    }
}
