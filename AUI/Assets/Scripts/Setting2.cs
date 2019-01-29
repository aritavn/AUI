using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;

public class Setting2 : MonoBehaviour, IInputClickHandler
{

    GameObject button;
    bool startCond = true;
    bool order = true;

    // Use this for initialization
    void Start()
    {
        button = GameObject.Find("Button1");
        PlayerPrefs.SetInt("order", 1);
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
            GameObject.Find("Text (4)").GetComponent<UnityEngine.UI.Text>().text = "Casuale";
            startCond = false;
            order = false;
            PlayerPrefs.SetInt("order", 0);
        }
        else
        {

            GameObject.Find("Text (4)").GetComponent<UnityEngine.UI.Text>().text = "In Ordine";
            order = true;
            startCond = true;
            PlayerPrefs.SetInt("order", 1);
        }

    }

}
