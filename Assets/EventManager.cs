using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Event {
    public Color color;
    public string name;
}

public class EventManager : MonoBehaviour {

    public static EventManager S;
    public List<Event> events;

    void Awake() {
        S = this;
    }


}
