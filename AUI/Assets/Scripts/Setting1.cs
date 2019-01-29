using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;

public class Setting1 : MonoBehaviour, IInputClickHandler
{

    GameObject button;
    bool startCond = true;
    bool hard = true;

    // Use this for initialization
    void Start () {
        button = GameObject.Find("Button1");
        PlayerPrefs.SetInt("hard", 1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        button.GetComponent<AudioSource>().Play();

        if (startCond) {
            GameObject.Find("Text (3)").GetComponent<UnityEngine.UI.Text>().text = "Facile";
            startCond = false;
            hard = false;
            PlayerPrefs.SetInt("hard", 0);
        }
        else
        {

            GameObject.Find("Text (3)").GetComponent<UnityEngine.UI.Text>().text = "Difficile";
            startCond = true;
            hard = true;
            PlayerPrefs.SetInt("hard", 1);
        }

    }

}
