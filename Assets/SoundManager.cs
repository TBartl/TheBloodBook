using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager S;
    public AudioClip clip;

	void Awake () {
        S = this;
	}

    public void Play() {
        AudioSource.PlayClipAtPoint(clip, Vector3.zero, 10f);
    }
}
