﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Event {
    public Color color;
    public Color colorText;
    public string name;
}

public class EventManager : MonoBehaviour {

    public static EventManager S;
    public List<Event> events;
    public GameObject eventBox;

    void Awake() {
        S = this;
    }


}
