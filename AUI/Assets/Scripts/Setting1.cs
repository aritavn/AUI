using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;

public class Setting1 : MonoBehaviour, IInputClickHandler
{

    GameObject button;
    bool startCond = true;
    bool easy = true;

    // Use this for initialization
    void Start () {
        button = GameObject.Find("Button1");
        PlayerPrefs.SetInt("easy", 1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        button.GetComponent<AudioSource>().Play();

        if (startCond) {
            GameObject.Find("Text (3)").GetComponent<UnityEngine.UI.Text>().text = "Difficile";
            startCond = false;
            easy = false;
            PlayerPrefs.SetInt("easy", 0);
        }
        else
        {

            GameObject.Find("Text (3)").GetComponent<UnityEngine.UI.Text>().text = "Facile";
            startCond = true;
            easy = true;
            PlayerPrefs.SetInt("easy", 1);
        }

    }

}
