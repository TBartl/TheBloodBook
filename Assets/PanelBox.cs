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

    protected override void Awake() {
        base.Awake();
        slidingPanel = this.transform.parent.parent.GetComponent<SlidingPanel>();
        rectTransform = this.GetComponent<RectTransform>();
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
                slidingPanel.StartSlide(originalMousePos.y);
                yield break;
            }
            yield return null;
        }
        this.transform.SetSiblingIndex(-1);
        if (InputManager.GetPosition().x < rectTransform.position.x + rectTransform.rect.width / 3f)
            StartCoroutine(ResizeThis(originalMousePos, false));
        else if (InputManager.GetPosition().x > rectTransform.position.x + rectTransform.rect.width * 2f / 3f)
            StartCoroutine(ResizeThis(originalMousePos, true));
        else
            StartCoroutine(MoveThis(originalMousePos));
        yield return null;
    }

    IEnumerator MoveThis(Vector2 originalMousePos) {
        float originalBoxPos = rectTransform.position.y;
        while (InputManager.IsHeld()) {
            Vector3 newPosition = rectTransform.position;
            newPosition.y = originalBoxPos + InputManager.GetPosition().y  - originalMousePos.y;
            rectTransform.position = newPosition;
            yield return null;
        }
        this.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
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
            rectTransform.sizeDelta = newSize;

            if (!downNotUp && newSize.y != minSize) {
                Vector3 newPosition = rectTransform.position;
                newPosition.y = originalBoxPos + (InputManager.GetPosition().y - originalMousePos.y);
                rectTransform.position = newPosition;
            }
            yield return null;
        }
    }

}
