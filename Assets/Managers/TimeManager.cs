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

    public static string folder;

    public RectTransform arrow;

    DateTime selectedDay;
    DateTime lastRealTime;

    // Use this for initialization
    void Awake () {
        S = this;
        if (!Directory.Exists(Application.persistentDataPath))
            Directory.CreateDirectory(Application.persistentDataPath);
        folder = Application.persistentDataPath + "/SaveData";
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        LoadDayAndPopulateBoxes(DateTime.Now);
    }

    void Update() {
        DateTime now = DateTime.Now;
        if (now.Minute != lastRealTime.Minute) {
            UpdateClockTime();
            Avian.S.OnMinuteUpdated();
        }
    }
    void UpdateClockTime() {
        DateTime now = DateTime.Now;
        bottomText.text = months[selectedDay.Month] + " " + selectedDay.Day + ", " + selectedDay.Year + " " + Utilities.GetFormattedTime(now.Hour, now.Minute);
        if (selectedDay.Date == now.Date)
            bottomText.color = Color.white;
        else
            bottomText.color = Color.gray;
        arrow.localPosition = new Vector3(0, -((now.Hour - 7) * 60 + now.Minute), 0);
        lastRealTime = now;
    }    

    void LoadDayAndPopulateBoxes(DateTime when) {
        // Delete current
        foreach (Transform child in timeSlotBoxHolder.transform) {
            GameObject.Destroy(child.gameObject);
        }

        TimeSlotData[] timeSlotData = LoadDayTimeSlots(when);
        foreach (TimeSlotData t in timeSlotData) {
            GameObject newBox = GameObject.Instantiate(EventManager.S.eventBox, timeSlotBoxHolder.transform.position, Quaternion.identity, timeSlotBoxHolder.transform);
            TimeSlot tSlot = newBox.GetComponent<TimeSlot>();
            tSlot.data = t;
            newBox.GetComponent<PanelBox>().SetNotFirstPlace();
            
        }
        selectedDay = when;
        UpdateClockTime();
    }
    public TimeSlotData[] LoadDayTimeSlots() {
        return LoadDayTimeSlots(DateTime.Now);
    }


    public TimeSlotData[] LoadDayTimeSlots(DateTime when) {
        string path = GetPath(when);
        if (!File.Exists(path)) {
            string templatePath = GetPathOfTemplate(when);
            if (File.Exists(templatePath))
                File.Copy(templatePath, path);
            else
                return new TimeSlotData[0];
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

    public void SaveDay(bool toTemplate = false) {
        StringBuilder sb = new StringBuilder();
        TimeSlot[] timeSlots = timeSlotBoxHolder.GetComponentsInChildren<TimeSlot>();
        sb.Append(timeSlots.Length + "\n");
        foreach (TimeSlot t in timeSlots) {
            sb.Append(t.data.id + "\n");
            sb.Append(t.data.startTime + "\n");
            sb.Append(t.data.duration + "\n");
        }
        if (!toTemplate)
            File.WriteAllText(GetPath(selectedDay), sb.ToString());
        else
            File.WriteAllText(GetPathOfTemplate(selectedDay), sb.ToString());
    }

    string GetPathOfTemplate(DateTime day) {
        string path = folder + "/template" + (int)day.DayOfWeek + ".txt";
        return path;
    }

    string GetPath(DateTime day) {
        string path = folder + "/" + day.Month + "_" + day.Day + "_" + day.Year + ".txt";
        return path;
    }

    public void ChangeDayToCurrent() {
        Avian.S.OnSelectCurrentDay(selectedDay.Date == DateTime.Now.Date);
        LoadDayAndPopulateBoxes(DateTime.Now);
    }

    public void ChangeDay(bool forward) {
        DateTime newDay;
        if (forward)
            newDay = selectedDay.AddDays(1);
        else
            newDay = selectedDay.AddDays(-1);

        if (newDay.Date == DateTime.Now.Date)
            Avian.S.OnBackToCurrentDay();
        else if (!forward)
            Avian.S.OnBackDay();
        else if (forward)
            Avian.S.OnForwardDay();

        LoadDayAndPopulateBoxes(newDay);
    }

    public DateTime GetSelectedDay() {
        return selectedDay;
    }

    public void SaveCurrentTemplate() {
        SaveDay(true);
    }

    public void ResetDayToTemplate() {
        File.Delete(GetPath(selectedDay));
        LoadDayAndPopulateBoxes(selectedDay);
    }
}
