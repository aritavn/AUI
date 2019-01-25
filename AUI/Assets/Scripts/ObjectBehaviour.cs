using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehaviour : MonoBehaviour, IInputClickHandler
{
    public void OnInputClicked(InputClickedEventData eventData)
    {
        int state = GameObject.Find("ChoiceManager").GetComponent<ChoiceManager>().getGameState();
        if(state == 5)
        {
            state = 1;
        }
        if (gameObject.GetComponent<ID>().getID()==state)
        {
            if (GameObject.Find("ChoiceManager").GetComponent<ChoiceManager>().getCompleted() == 0)
            {
                GameObject.Find("ChoiceManager").GetComponent<ChoiceManager>().setCompleted();
                GameObject.Find("Box8").GetComponent<Roulette>().showRight(0);
                GameObject.Find("Box8").GetComponent<Roulette>().playRight();
                StartCoroutine(Wait());
                GameObject.Find("ChoiceManager").GetComponent<ChoiceManager>().DestroyObjects();
            }
        }
        else
        {
            GameObject.Find("Box8").GetComponent<Roulette>().showWrong();
            GameObject.Find("Box8").GetComponent<Roulette>().playWrong();
        }
          
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        GameObject.Find("Box8").GetComponent<Roulette>().stopRoulette();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
