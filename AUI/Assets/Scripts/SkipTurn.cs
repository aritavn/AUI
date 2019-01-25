using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class SkipTurn : MonoBehaviour, IInputClickHandler
{

    GameObject button;
    private Roulette roulette;

    // Use this for initialization
    void Start () {
        button = GameObject.Find("Skip");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        button.GetComponent<AudioSource>().Play();
        Debug.Log("clicchete");
        roulette = GameObject.Find("Box8").GetComponent<Roulette>();
        roulette.skipTurn();

    }

}
