using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelBox : Selectable {
    SlidingPanel slidingPanel;
    static float holdTime = .15f;
    static float moveThreshold = 10f;
    RectTransform rectTransform;
    int minSize = 10;
    TimeSlot timeSlot;

    protected override void Awake() {
        base.Awake();
        slidingPanel = this.transform.GetComponentInParent<SlidingPanel>();
        rectTransform = this.GetComponent<RectTransform>();
        timeSlot = this.GetComponent<TimeSlot>();
    }

    public override void OnPointerDown(PointerEventData eventData) {
        base.OnPointerDown(eventData);
        StartCoroutine(StartSlideOrMoveThis());
    }

    IEnumerator StartSlideOrMoveThis() {
        Vector2 originalMousePos = InputManager.GetPosition();
        for (float t = 0; t < holdTime; t += Time.deltaTime) {
            if (!InputManager.IsHeld()) {
                yield break;
            }
            if (Vector2.Distance(originalMousePos, InputManager.GetPosition()) > moveThreshold) {
                if (slidingPanel)
                    slidingPanel.StartSlide(originalMousePos.y);
                yield break;
            }
            yield return null;
        }
        this.transform.SetSiblingIndex(-1);
        if (InputManager.GetPosition().x < rectTransform.position.x - rectTransform.rect.width / 6f)
            StartCoroutine(ResizeThis(originalMousePos, false));
        else if (InputManager.GetPosition().x > rectTransform.position.x + rectTransform.rect.width / 6f)
            StartCoroutine(ResizeThis(originalMousePos, true));
        else
            StartCoroutine(MoveThis(originalMousePos));
        yield return null;
    }

    public IEnumerator MoveThis(Vector2 originalMousePos) {
        
        float originalBoxPos = rectTransform.position.y;
        RectTransform currentPanel = GetCurrentPanel();
        while (InputManager.IsHeld()) {

            Vector3 newPosition = rectTransform.position;   
            newPosition.y = originalBoxPos + InputManager.GetPosition().y  - originalMousePos.y;
            newPosition.y = Mathf.Clamp(newPosition.y, currentPanel.position.y - currentPanel.sizeDelta.y + rectTransform.sizeDelta.y, currentPanel.position.y);
            newPosition.x = currentPanel.position.x;
            rectTransform.position = newPosition;
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, Mathf.Round(rectTransform.localPosition.y / 5) * 5);
            this.transform.SetParent(currentPanel, true);

            currentPanel = GetCurrentPanel();
            timeSlot.SetFromRect();
            yield return null;
        }
        if (currentPanel == PanelManager.S.panelA)
            DestroyImmediate(this.gameObject);
        else {
            this.transform.SetParent(PanelManager.S.panelB, true);
            slidingPanel = this.transform.GetComponentInParent<SlidingPanel>();
        }
        TimeManager.S.SaveDay();
    }

    RectTransform GetCurrentPanel() {
        float distToA = Mathf.Abs(InputManager.GetPosition().x - PanelManager.S.panelA.position.x);
        float distToB = Mathf.Abs(InputManager.GetPosition().x - PanelManager.S.panelB.position.x);
        if (distToA < distToB)
            return PanelManager.S.panelA;
        return PanelManager.S.panelB;
    }

    IEnumerator ResizeThis(Vector2 originalMousePos, bool downNotUp) {
        float originalBoxHeight = rectTransform.sizeDelta.y;
        float originalBoxPos = rectTransform.position.y;
        while (InputManager.IsHeld()) {
            Vector2 newSize = rectTransform.sizeDelta;
            if (downNotUp)
                newSize.y = Mathf.Max(minSize, originalBoxHeight - (InputManager.GetPosition().y - originalMousePos.y));
            else
                newSize.y = Mathf.Max(minSize, originalBoxHeight + (InputManager.GetPosition().y - originalMousePos.y));
            newSize.y = Mathf.Round(newSize.y / 5) * 5;
            rectTransform.sizeDelta = newSize;

            if (!downNotUp && newSize.y != minSize) {
                Vector3 newPosition = rectTransform.position;
                newPosition.y = originalBoxPos + (InputManager.GetPosition().y - originalMousePos.y);
                newPosition.y = Mathf.Round(newPosition.y / 5) * 5;
                rectTransform.position = newPosition;
                rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, Mathf.Round(rectTransform.localPosition.y / 5) * 5);
            }
            timeSlot.SetFromRect();
            yield return null;
        }
        TimeManager.S.SaveDay();
    }

}
