using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour {

    public static PanelManager S;

    public RectTransform panelA;
    public RectTransform panelB;

	// Use this for initialization
	void Awake () {
        S = this;
	}
}
