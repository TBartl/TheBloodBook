using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class DateButton : Selectable {

    float holdTime = .4f;
    float resetTime = 6f;
    bool alreadyGoing = false;

    bool thisPressed = false;

    public override void OnPointerDown(PointerEventData eventData) {
        base.OnPointerDown(eventData);
        if (!alreadyGoing)
            StartCoroutine(ButtonPressed());
        StartCoroutine(ThisPressed());
    }

    IEnumerator ThisPressed() {
        thisPressed = true;
        yield return null;
        thisPressed = false;
    }


    IEnumerator ButtonPressed() {
        alreadyGoing = true;
        for (float t = 0; t < holdTime; t += Time.deltaTime) {
            if (!InputManager.IsHeld()) {
                TimeManager.S.ChangeDayToCurrent();
                alreadyGoing = false;
                yield break;
            }
            yield return null;
        }

        while (InputManager.IsHeld())
            yield return null;
        Avian.S.OnMasterButton();

        bool toReset = true;
        for (float t = 0; t < resetTime; t += Time.deltaTime) {
            if (thisPressed) {
                toReset = false;
                break;
            }
            yield return null;
        }
        if (toReset) {
            Avian.S.OnMasterButtonFizzled();
            alreadyGoing = false;
            yield break;
        }

        for (float t = 0; t < holdTime; t += Time.deltaTime) {
            if (!InputManager.IsHeld()) {
                TimeManager.S.ResetDayToTemplate();
                Avian.S.OnLoadedTemplate();
                alreadyGoing = false;
                yield break;
            }
            yield return null;
        }
        TimeManager.S.SaveCurrentTemplate();
        Avian.S.OnSavedTemplate();
        alreadyGoing = false;
    }
}
