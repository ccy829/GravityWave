using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineZero : MonoBehaviour {
    RectTransform rectPanel;
    LineRenderer line;
    float panelWidth_half;

    void Awake() {
        rectPanel = GetComponent<RectTransform>();
        line = GetComponent<LineRenderer>();
        panelWidth_half = rectPanel.rect.width / 2.0f;
        line.numPositions = 2;
    }

	// Use this for initialization
	void Start () {
        line.SetPosition(0, new Vector3(-panelWidth_half, 0, 0));
        line.SetPosition(1, new Vector3(panelWidth_half, 0, 0));
    }

    public void hideMonitor() {
        GetComponent<Image>().enabled = false;
        line.enabled = false;
    }

    public void showMonitor() {
        GetComponent<Image>().enabled = true;
        line.enabled = true;
    }
}
