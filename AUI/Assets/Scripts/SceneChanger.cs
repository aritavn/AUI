using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour, IInputClickHandler
{
    GameObject basics;
    GameObject music;
	// Use this for initialization
	void Start () {
        basics = GameObject.Find("Basics");
        music = GameObject.Find("MainSound");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("click");
        ChangeScene("MainScene");
    }

    public void ChangeScene(string sceneName) {
        
        Application.LoadLevel(sceneName);
        Destroy(basics);
        DontDestroyOnLoad(music);
    }
}
