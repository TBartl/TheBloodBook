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
    Text[] text;
    RectTransform rectTransform;
    public bool printDurationWithTitle = true;

    void Awake() {
        text = this.GetComponentsInChildren<Text>();
        rectTransform = this.GetComponent<RectTransform>();
    }

	// Use this for initialization
	void Start () {
        if (data.startTime == 0) {
            SetFromRect();
        }

        UpdateSize();
        UpdatePos();
        UpdateColor();
        UpdateText();
    }

    public void SetFromRect() {
        data.duration = Mathf.RoundToInt(rectTransform.sizeDelta.y);
        UpdateText();
        data.startTime = -Mathf.RoundToInt(rectTransform.localPosition.y);
    }

    void UpdateSize() {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, data.duration);
    }
    void UpdatePos() {
        rectTransform.localPosition = new Vector2(0, -data.startTime);
    }

    void UpdateColor() {
        this.GetComponentInChildren<Image>().color = EventManager.S.events[data.id].color ;
    }

    void UpdateText() {
        if (printDurationWithTitle) {
            //text[0].text = "(" + data.duration.ToString() + ")" + " " + EventManager.S.events[data.id].name;
            text[0].text = EventManager.S.events[data.id].name + " (" + data.duration.ToString() + ")";
            int hour = data.startTime / 60 + 7;
            int minute = data.startTime % 60;
            text[1].text = Utilities.GetFormattedTime(hour, minute);
        }
        else
            text[0].text = EventManager.S.events[data.id].name;
    }
    
}
