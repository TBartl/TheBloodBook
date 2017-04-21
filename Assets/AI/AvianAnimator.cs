using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct AvianAnimation {
    public List<Sprite> sprites;
    public bool repeating;
}

[System.Serializable]
public struct AvianSpeech {
    public string text;
    public AvianAnimation talking;

    public AvianSpeech(string text, AvianAnimation talking) {
        this.text = text;
        this.talking = talking;
    }
}

public class AvianAnimator : MonoBehaviour {

    Image image;

    float frameTime = .10f;
    float characterTime = .015f;
    float spaceCharacterTime = .025f;
    float endTextWaitTime = 1.5f;
    public AvianAnimation currentAnimation;
    Coroutine updateFrames;

    int currentFrame = 0;
    List<AvianSpeech> speechQueue = new List<AvianSpeech>();

    public Text speechText;

    void Start() {
        image = this.GetComponent<Image>();
        StartCoroutine(RunSpeech());
        ChangeToAnimation(currentAnimation);
    }    

    public void AnimateOutSpeech(AvianSpeech speech) {
        if (speechQueue.Count < 3)
            speechQueue.Add(speech);
    }

    void ChangeToAnimation(AvianAnimation newAnimation) {
        currentAnimation = newAnimation;
        currentFrame = currentFrame % currentAnimation.sprites.Count;
        SetFrame();
    }

    void SetFrame() {
        image.sprite = currentAnimation.sprites[currentFrame];
    }

    IEnumerator UpdateFrames() {
        while (true) {
            yield return new WaitForSeconds(frameTime);
            currentFrame = (currentFrame + 1) % currentAnimation.sprites.Count;
            SetFrame();
        }
    }

    IEnumerator RunSpeech() {
        while (true) {
            if (speechQueue.Count > 0) {
                AvianSpeech currentSpeach = speechQueue[0];
                ChangeToAnimation(currentSpeach.talking);
                speechQueue.RemoveAt(0);

                if (currentSpeach.talking.repeating)
                    updateFrames = StartCoroutine(UpdateFrames());

                // Get just the text to put at the end so spacing works out.
                string fakeString = "";
                for (int i = 0; i < currentSpeach.text.Length; i++) {
                    char c = currentSpeach.text[i];
                    if (c == '<') {
                        while (c != '>') {
                            i += 1;
                            c = currentSpeach.text[i];
                        }
                    }
                    else {
                        fakeString += c;
                    }
                }
                
                                
                int charIndex = 0;
                int charIndexFake = 0; // Doesn't account for 
                string builtString = "";
                bool colorStarted = false;
                while (charIndex < currentSpeach.text.Length) {
                    char c = currentSpeach.text[charIndex];
                    builtString += c;
                    if (c == '<') {
                        colorStarted = !colorStarted;
                        while (c != '>') {
                            charIndex += 1;
                            c = currentSpeach.text[charIndex];
                            builtString += c;
                        }
                        charIndex += 1;
                    } else {
                        charIndex += 1;
                        charIndexFake += 1;
                    }
                    string invisSuffix = "";
                    if (charIndex < currentSpeach.text.Length)
                        invisSuffix = ColorStartInvis() + fakeString.Substring(charIndexFake) + ColorEnd();
                    if (colorStarted)
                        speechText.text = builtString + ColorEnd() + invisSuffix;
                    else
                        speechText.text = builtString + invisSuffix;


                    if (!currentSpeach.talking.repeating) {
                        currentFrame =  Mathf.FloorToInt(charIndexFake / (float) fakeString.Length * (currentAnimation.sprites.Count - 1));
                        SetFrame();
                    }

                        if (c == ' ')
                        yield return new WaitForSeconds(spaceCharacterTime);
                    else
                        yield return new WaitForSeconds(characterTime);
                }
                if (currentSpeach.talking.repeating)
                    StopCoroutine(updateFrames);
                yield return new WaitForSeconds(endTextWaitTime);
            }
            yield return null;
        }
    }

    public string ColorStartInvis() {
        return "<color=#00000000>";
    }

    public string ColorEnd() {
        return "</color>";
    }
}
