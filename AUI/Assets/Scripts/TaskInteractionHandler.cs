using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskInteractionHandler : MonoBehaviour, ISpeechHandler, IInputClickHandler
{

    private bool readyToPlay;
    GameObject box;
    GameObject rocket;

    // Use this for initialization
    void Start()
    {
        box = GameObject.Find("Box8");
        rocket = GameObject.Find("Rocket");

        readyToPlay = false;
        box.SetActive(false);
        rocket.GetComponent<ParticleSystem>().Pause();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        //throw new System.NotImplementedException();
    }


    public void SetMenuVisible()
    {
        //transform.GetChild(2).gameObject.SetActive(true);
    }

    public void ScanningComplete()
    {
        readyToPlay = true;
        // TaskManager.Instance.GenerateObjectsInWorld();
        // transform.GetChild(0).gameObject.SetActive(false);
        //transform.GetChild(1).gameObject.SetActive(true);

        StartCoroutine(Wait());
    }

    public void StartPlay()
    {
       /* if (readyToPlay)
        {
            TaskManager.Instance.GenerateObjectsInWorld();
            readyToPlay = false;
        }*/
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        box.SetActive(true);
        rocket.GetComponent<ParticleSystem>().Pause();
        //TaskManager.Instance.GenerateObjectsInWorld();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        //StartPlay();
    }


}
