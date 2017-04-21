using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortraitUpsideDown = false;
        //Application.runInBackground = true;
    }
}
