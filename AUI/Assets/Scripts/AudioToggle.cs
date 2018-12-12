﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class AudioToggle : MonoBehaviour, IInputClickHandler
{

    private GameObject music;
    private bool isPlaying=true;
    // Use this for initialization
    void Start () {
        music = GameObject.Find("MainSound");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (isPlaying) {
            music.GetComponent<AudioSource>().Stop();
            isPlaying = false;
        }
        else
        {
            music.GetComponent<AudioSource>().Play();
            isPlaying = true;
        }

    }

}
