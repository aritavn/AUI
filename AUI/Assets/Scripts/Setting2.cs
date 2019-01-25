using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;

public class Setting2 : MonoBehaviour, IInputClickHandler
{

    GameObject button;
    bool startCond = true;
    bool random = true;

    // Use this for initialization
    void Start()
    {
        button = GameObject.Find("Button1");
        PlayerPrefs.SetInt("random", 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        button.GetComponent<AudioSource>().Play();

        if (startCond)
        {
            GameObject.Find("Text (4)").GetComponent<UnityEngine.UI.Text>().text = "In Ordine";
            startCond = false;
            random = false;
            PlayerPrefs.SetInt("random", 0);
        }
        else
        {

            GameObject.Find("Text (4)").GetComponent<UnityEngine.UI.Text>().text = "Casuale";
            random = true;
            startCond = true;
            PlayerPrefs.SetInt("random", 1);
        }

    }

}
