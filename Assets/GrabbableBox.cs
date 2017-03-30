using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GrabbableBox : Selectable {
    public int id;
    static float holdTime = .15f;

    protected override void Awake() {
        base.Awake();
    }

    public override void OnPointerDown(PointerEventData eventData) {
        base.OnPointerDown(eventData);
        StartCoroutine(TryPickThisUp());
    }
    
    IEnumerator TryPickThisUp() {
        Vector2 originalMousePos = InputManager.GetPosition();
        for (float t = 0; t < holdTime; t += Time.deltaTime) {
            if (!InputManager.IsHeld()) {
                yield break;
            }
            yield return null;
        }
        Debug.Log("Picked up " + EventManager.S.events[id].name);
    }

}
