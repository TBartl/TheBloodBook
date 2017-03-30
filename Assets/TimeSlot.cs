using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TimeSlotData {
    public int id = 0;
    public int duration = 30;
    public int startTime = 0;
}

public class TimeSlot : MonoBehaviour {

    public TimeSlotData data;
    Text text;
    RectTransform rectTransform;
    public bool printDurationWithTitle = true;

	// Use this for initialization
	void Start () {
        text = this.GetComponentInChildren<Text>();
        rectTransform = this.GetComponent<RectTransform>();
        if (data.startTime == 0) {
            SetDurationFromRect();
            SetStartFromRect();
        }

        UpdateSize();
        UpdatePos();
        UpdateColor();
        UpdateText();
    }

    public void SetDurationFromRect() {
        data.duration = Mathf.RoundToInt(rectTransform.sizeDelta.y);
        UpdateText();
    }
    public void SetStartFromRect() {
        data.startTime = Mathf.RoundToInt(rectTransform.localPosition.y);
    }

    void UpdateSize() {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, data.duration);
    }
    void UpdatePos() {
        rectTransform.localPosition = new Vector2(0, data.startTime);
    }

    void UpdateColor() {
        this.GetComponentInChildren<Image>().color = EventManager.S.events[data.id].color ;
        text.text = "(" + data.duration.ToString() + ")" + " " + EventManager.S.events[data.id].name;
    }

    void UpdateText() {
        if (printDurationWithTitle)
            text.text = "(" + data.duration.ToString() + ")" + " " + EventManager.S.events[data.id].name;
        else
            text.text = EventManager.S.events[data.id].name;
    }
}
