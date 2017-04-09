using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.IO;

public class TimeManager : MonoBehaviour {
    public static TimeManager S;

    public Text bottomText;
    public GameObject timeSlotBoxHolder;
    string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

    public static string folder = "SaveData";

    DateTime selectedDay;
    DateTime lastRealTime;

    // Use this for initialization
    void Awake () {
        S = this;
        LoadDayAndPopulateBoxes(DateTime.Now);
        UpdateClockTime();
    }

    void Update() {
        DateTime now = DateTime.Now;
        if (now.Minute != lastRealTime.Minute) {
            UpdateClockTime();
        }
    }
    void UpdateClockTime() {
        DateTime now = DateTime.Now;
        bottomText.text = months[now.Month] + " " + now.Day + ", " + now.Year + " " + Utilities.GetFormattedTime(now.Hour, now.Minute);
        lastRealTime = now;
    }


    

    void LoadDayAndPopulateBoxes(DateTime when) {
        //GameObject newBox = GameObject.Instantiate(EventManager.S.eventBox, timeSlotBoxHolder.transform.position, Quaternion.identity, timeSlotBoxHolder.transform);
        //PanelBox newPanelBox = newBox.GetComponent<PanelBox>();

        // Delete all current objects

        //Check loaded
        //   load from file
        //   create new file from template

        selectedDay = when;
    }

    public TimeSlotData[] LoadDayTimeSlots(DateTime when) {
        string path = GetPath(when);
        if (!File.Exists(path)) {
            string templatePath = GetPathOfTemplate(when);
            if (!File.Exists(templatePath))
                return new TimeSlotData[0];
            else
                File.Copy(templatePath, path);
        }
        string[] lines = File.ReadAllLines(path);
        int numEntries = int.Parse(lines[0]);
        TimeSlotData[] toReturn = new TimeSlotData[numEntries];
        for (int i = 0; i < numEntries; i++) {
            toReturn[i].id = int.Parse(lines[i * 3 + 1]);
            toReturn[i].startTime = int.Parse(lines[i * 3 + 2]);
            toReturn[i].duration = int.Parse(lines[i * 3 + 3]);
        }
        return toReturn;
    }

    public void SaveDay() {
        StringBuilder sb = new StringBuilder();
        TimeSlot[] timeSlots = timeSlotBoxHolder.GetComponentsInChildren<TimeSlot>();
        sb.Append(timeSlots.Length + "\n");
        foreach (TimeSlot t in timeSlots) {
            sb.Append(t.data.id + "\n");
            sb.Append(t.data.startTime + "\n");
            sb.Append(t.data.duration + "\n");
        }
        File.WriteAllText(GetPath(selectedDay), sb.ToString());
    }

    string GetPathOfTemplate(DateTime day) {
        string path = folder + "/template" + (int)day.DayOfWeek + ".txt";
        return path;
    }

    string GetPath(DateTime day) {
        string path = folder + "/" + selectedDay.Month + "_" + selectedDay.Day + "_" + selectedDay.Year + ".txt";
        return path;
    }
}
