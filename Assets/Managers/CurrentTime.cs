using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CurrentTime : MonoBehaviour {

    Text text;
    DateTime last;
    string[] months = {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    DateTime overrideTime;

    void Awake() {
        text = this.GetComponent<Text>();
        overrideTime = DateTime.Now;
        UpdateTime();
    }

    void Update() {
        DateTime now = DateTime.Now;
        if (Input.GetKey(KeyCode.Space))
            overrideTime.AddMinutes(1);
        now = overrideTime;
        if (now.Minute != last.Minute) {
            UpdateTime();
        }
    }

    void UpdateTime() {
        DateTime now = DateTime.Now;
        text.text = months[now.Month] + " " + now.Day + ", " + now.Year + " " + GetFormattedTime(now);
        last = now;
    }

    public static string GetFormattedTime(DateTime time) {
        int hour = time.Hour;
        int minute = time.Minute;
        string ap = "AM";
        if (hour > 11)
            ap = "PM";

        if (hour > 12)
            hour = hour - 12;
        if (hour == 0)
            hour = 12;

        string minuteString = minute.ToString();
        if (minute < 10)
            minuteString = "0" + minuteString;

        return hour.ToString() + ':' + minuteString + ap;
    }
}
