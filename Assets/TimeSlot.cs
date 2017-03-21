using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct TimeSlotData {
    public string name;
    public int duration;
    public int startTime;
}

public class TimeSlot : MonoBehaviour {

    TimeSlotData data;
    Text text;
    RectTransform rectTransform;

	// Use this for initialization
	void Start () {
        text = this.GetComponentInChildren<Text>();
        rectTransform = this.GetComponent<RectTransform>();
        if (data.duration == 0)
            SetDataFromRect();

        UpdateText();
        UpdateSize();
        UpdatePos();
	}

    void SetDataFromRect() {
        data.name = text.text;
        data.duration = Mathf.RoundToInt(rectTransform.sizeDelta.y);
        Debug.Log(rectTransform.localPosition);
        data.startTime = Mathf.RoundToInt(rectTransform.localPosition.y);
    }

    public void UpdateText() {
        text.text = "(" + data.duration.ToString() + ")" + " " + data.name;
    }
    public void UpdateSize() {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, data.duration);
    }
    public void UpdatePos() {
        rectTransform.localPosition = new Vector2(0, data.startTime);
    }
}
