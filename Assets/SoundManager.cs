using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource[] soundList;


	// Use this for initialization
	void Awake () {
        soundList = GetComponentsInChildren<AudioSource>();        
	}

    public void PlaySound(int index) {
        soundList[index].Play();
    }
}
