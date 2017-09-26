using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlidingPanel : Selectable {
    RectTransform rectTransform;
    RectTransform parentTransform;

    int maxVelocities = 3;
    Queue<float> velocities = new Queue<float>();  
    float velocity;
    static float acceleration = 2000f;

    protected override void Awake() {
        base.Awake();
        rectTransform = this.GetComponent<RectTransform>();
        parentTransform = transform.parent.GetComponent<RectTransform>();
    }

    public override void OnPointerDown(PointerEventData eventData) {
        base.OnPointerDown(eventData);
        StartSlide();
    }

    public void StartSlide(float yOverride = -1) {
        StartCoroutine(Slide(yOverride));
    }

    IEnumerator Slide(float yOverride = -1) {
        float lastPos = InputManager.GetPosition().y;
        if (yOverride != -1) // If we're starting the slide from a Panel box, we still want to move from where the finger was put down.
            lastPos = yOverride;
        while (InputManager.IsHeld()) {
            float newPos = InputManager.GetPosition().y;
            float diff = newPos - lastPos;
            velocities.Enqueue(diff / Time.deltaTime);
            if (velocities.Count > maxVelocities)
                velocities.Dequeue();
            float newY = rectTransform.anchoredPosition.y + diff;
            ClampOnPosition(newY);
            lastPos = newPos;
            yield return null;
        }
        StartCoroutine(Drift());
    }

    IEnumerator Drift() {
        int velocityCount = velocities.Count;
        velocity = 0;
        while (velocities.Count > 0) {
            velocity += velocities.Dequeue();
        }
        velocity = velocity / velocityCount;

        while (velocity != 0) {
            if (InputManager.IsHeld())
                yield break;
            float newY = rectTransform.anchoredPosition.y + velocity * Time.deltaTime;
            ClampOnPosition(newY);
            if (velocity > 0) {
                velocity = Mathf.Max(0, velocity - acceleration * Time.deltaTime);
            } 
            else {
                velocity = Mathf.Min(0, velocity + acceleration * Time.deltaTime);
            }   
            yield return null;
        }
        this.transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y), 0);
    }

    void ClampOnPosition(float y) {
        float minPos = 0;
        float maxPos = rectTransform.sizeDelta.y - parentTransform.sizeDelta.y;
        if (y < minPos) {
            y = minPos;
            velocity = 0;
        }
        else if (y > maxPos) {
            y = maxPos;
            velocity = 0;
        }
        rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x, y);
    }
}
