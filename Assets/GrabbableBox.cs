using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GrabbableBox : Selectable {
    public int id;
    static float holdTime = .15f;
    RectTransform rectTransform;
    TimeSlot timeSlot;

    protected override void Awake() {
        base.Awake();
        rectTransform = this.GetComponent<RectTransform>();
        timeSlot = this.GetComponent<TimeSlot>();
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
        GameObject newBox = GameObject.Instantiate(EventManager.S.eventBox, rectTransform.position, Quaternion.identity, rectTransform.parent);
        PanelBox newPanelBox = newBox.GetComponent<PanelBox>();
        newPanelBox.StartCoroutine(newPanelBox.MoveThis(originalMousePos));
        newPanelBox.GetComponent<TimeSlot>().data.id = timeSlot.data.id;
    }
}
