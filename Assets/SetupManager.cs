using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupManager : MonoBehaviour {

    bool hasFocus = true;

    public static SetupManager S;
    // Use this for initialization
    void Awake() {
        S = this;
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //Application.runInBackground = true;
    }

    void OnApplicationFocus(bool hasFocus) {
        this.hasFocus = hasFocus;
    }

    bool HasFocus() {
        return hasFocus;
    }
}
