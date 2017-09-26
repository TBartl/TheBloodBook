using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities {
    public static string GetFormattedTime(int hour, int minute) {
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
