using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityCurve : MonoBehaviour {
    public AnimationCurve value;
    public Image cursor;
    public int numSample;
    public int numKeyframe;
    public float speed;
    public float delay;
    float pct = 0;
    RectTransform rectPanel;
    Keyframe curveStart, curveEnd;
    static float panelWidth_half;
    static float curValue = 0;
    static float amp;
    static bool isPlaying = false;
    static LineRenderer curve;

    void Awake() {
        rectPanel = GetComponent<RectTransform>();
        curve = GetComponent<LineRenderer>();
        if (numSample < 2) numSample = 2;
        if (speed < 0) speed = 0.5f;
        else if (speed > 1.0f) speed = 1.0f;
        if (delay < 0) delay = 0.5f;
        else if (delay > 1.0f) delay = 1.0f;
        panelWidth_half = rectPanel.rect.width / 2.0f;
    }

	// Use this for initialization
	void Start () {
        amp = rectPanel.rect.height / 2.0f;
        curve.numPositions = numSample;
        curveStart = new Keyframe(0, 0);
        curveEnd = new Keyframe(1.0f, 0);
        value = new AnimationCurve(curveStart, curveEnd);
        value.preWrapMode = WrapMode.PingPong;
        value.postWrapMode = WrapMode.PingPong;
        generateKeyFrame();
        drawCurve();
        isPlaying = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (isPlaying) curValue = updateValue(Time.deltaTime);
        updateCurve();
    }

    void generateKeyFrame() {
        float newTime;
        float newValue;
        for (int i = 0; i < numKeyframe; i++) {
            newTime = Random.Range(0.1f, 0.9f);
            newValue = Random.Range(-1.0f, 1.0f);
            value.AddKey(newTime, newValue);
        }
    }

    void refreshKeyFrame() {
        float newTime;
        float newValue;

        curveStart.value = curveEnd.value;
        curveEnd.value = Random.Range(-1.0f, 1.0f);
        while (value.length > 2) {
            value.RemoveKey(1);
        }

        for (int i = 0; i < numKeyframe; i++) {
            newTime = Random.Range(0.1f, 0.9f);
            newValue = Random.Range(-1.0f, 1.0f);
            value.AddKey(newTime, newValue);
        }
    }

    void drawCurve()
    {
        float temp_pct = 0;
        Vector3 pos;
        for (int i = 0; i < curve.numPositions; i++)
        {
            temp_pct = (float)i / (float)(curve.numPositions - 1);
            if (temp_pct >= delay) curValue = updateValue(Time.deltaTime);
            pos = new Vector3(Mathf.Lerp(-panelWidth_half, panelWidth_half, temp_pct), curValue * amp, 0);
            curve.SetPosition(i, pos);
        }
        cursor.rectTransform.localPosition = new Vector3(rectPanel.rect.xMin, curve.GetPosition(0).y, 0);
    }

    void updateCurve() {
        for (int i = 0; i < curve.numPositions - 1; i++)
        {
            var newPosition = curve.GetPosition(i);
            newPosition.y = curve.GetPosition(i + 1).y;
            curve.SetPosition(i, newPosition);
        }
        curve.SetPosition(curve.numPositions - 1, new Vector3(panelWidth_half, curValue * amp, 0));
        cursor.rectTransform.localPosition = new Vector3(rectPanel.rect.xMin, curve.GetPosition(0).y, 0);
    }

    float updateValue(float timeElapse) {
        float temp;
        if (speed < 0) speed = 0.5f;
        else if (speed > 1.0f) speed = 1.0f;

        if (pct < 1.0f)
        {
            pct += (timeElapse * speed);
            temp = value.Evaluate(pct);
        }
        else
        {
            pct += (timeElapse * speed) - 1.0f;
            temp = value.Evaluate(pct);
            
            refreshKeyFrame();
        }
        if (temp > 1.0f) temp = 1.0f;
        else if (temp < -1.0f) temp = -1.0f;
        return temp;
    }

    public void hideCurve() {
        cursor.enabled = false;
        curve.enabled = false;
    }

    public void showCurve() {
        cursor.enabled = true;
        curve.enabled = true;
    }

    public static float getValue() {
        return (curve.GetPosition(0).y / amp);
    }

    public static void Pause() {
        isPlaying = false;
    }

    public static void Play() {
        isPlaying = true;
    }

    public static void Stop() {
        Pause();
        curValue = 0;
    }

    public static void Reset() {
        float temp_pct = 0;
        Vector3 pos;
        for (int i = 0; i < curve.numPositions; i++)
        {
            temp_pct = (float)i / (float)(curve.numPositions - 1);
            pos = new Vector3(Mathf.Lerp(-panelWidth_half, panelWidth_half, temp_pct), 0, 0);
            curve.SetPosition(i, pos);
        }
    }
}
