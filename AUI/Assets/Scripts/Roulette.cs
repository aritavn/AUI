﻿using HoloToolkit.UX.Progress;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class Roulette : MonoBehaviour {
    private GameObject ruota;
    private GameObject box;
    private Animator m_Animator;
    private GameObject rocket;
    private GameObject top;
    private GameObject left;
    private bool closing = false;

    public AudioClip clipRight;
    public AudioClip clipWrong;
    public AudioClip clipLever;
    public AudioClip clipRolling;
    public AudioClip clipFireworks;

    private AudioSource audioRight;
    private AudioSource audioWrong;
    private AudioSource audioLever;
    private AudioSource audioRolling;
    private AudioSource audioFireworks;
    private System.Random rnd = new System.Random();
    List<int> history = new List<int>();
    private bool ready = false;

    private int state = 0;
    private int permaState;
	// Use this for initialization
	void Start () {

        clipRight = Resources.Load("sounds/correct") as AudioClip;
        clipWrong = Resources.Load("sounds/wrong") as AudioClip;
        clipLever = Resources.Load("sounds/lever") as AudioClip;
        clipRolling = Resources.Load("sounds/spinning") as AudioClip;
        clipFireworks = Resources.Load("sounds/firework") as AudioClip;

        audioRight = AddAudio(clipRight, false, false, 0.45f, 1f);
        audioWrong = AddAudio(clipWrong, false, false, 0.7f, 1f);
        audioLever = AddAudio(clipLever, false, false, 0.5f, 1f);
        audioRolling = AddAudio(clipRolling, false, false, 0.6f, 1.4f);
        audioFireworks = AddAudio(clipFireworks, false, false, 0.1f, 0.95f);

        ruota = GameObject.Find("pCylinder7");
        left = GameObject.Find("pCube5");
        top = GameObject.Find("pCube3");
        box = GameObject.Find("Box8");
        m_Animator = box.GetComponent<Animator>();

        Destroy(box.GetComponent<Rigidbody>());

    }
	
	// Update is called once per frame
	void Update () {

        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Open") &&
            m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f &&
            closing==false)
        {
            setGo();
            switch (state)
            {
                case 1:
                    ruota.GetComponent<Renderer>().material = Resources.Load("Ruota 1") as Material;
                    break;
                case 2:
                    ruota.GetComponent<Renderer>().material = Resources.Load("Ruota 2") as Material;
                    break;
                case 3:
                    ruota.GetComponent<Renderer>().material = Resources.Load("Ruota 3") as Material;
                    break;
                case 4:
                    ruota.GetComponent<Renderer>().material = Resources.Load("Ruota 4") as Material;
                    break;
                case 5:
                    ruota.GetComponent<Renderer>().material = Resources.Load("Ruota 5") as Material;
                    break;
                case 6:
                    ruota.GetComponent<Renderer>().material = Resources.Load("Ruota 6") as Material;
                    break;
                case 7:
                    ruota.GetComponent<Renderer>().material = Resources.Load("Ruota 7") as Material;
                    break;
                case 8:
                    ruota.GetComponent<Renderer>().material = Resources.Load("Ruota 12") as Material;
                    ProgressIndicator.Instance.Open(
                            IndicatorStyleEnum.None,
                            ProgressStyleEnum.ProgressBar,
                            ProgressMessageStyleEnum.Visible,
                            "",
                            null);
                    break;
                case 9:
                    ruota.GetComponent<Renderer>().material = Resources.Load("Ruota 13") as Material;
                    ProgressIndicator.Instance.Open(
                            IndicatorStyleEnum.None,
                            ProgressStyleEnum.ProgressBar,
                            ProgressMessageStyleEnum.Visible,
                            "",
                            null);
                    break;
                default:
                    break;
            }
        }
    }

    public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol, float pitch)
    {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.clip = clip; 
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = vol;
        newAudio.pitch = pitch;
        return newAudio; 
    }

    public int getState()
    {
        return state;
    }

    private void randomState()
    {
        if (PlayerPrefs.GetInt("order") == 1)
        {
            bool isInList = false;
            if (history.Count == 9)
            {
                Destroy(GameObject.Find("Basics"));
                Destroy(GameObject.Find("Box8"));
                Destroy(GameObject.Find("Spatial"));
                Destroy(GameObject.Find("MainSound"));
                Destroy(GameObject.Find("ChoiceManager"));
                Application.LoadLevel("Menu");

            }
            else
            {
                do
                {
                    state = rnd.Next(1, 10);
                    isInList = history.IndexOf(state) != -1;
                } while (isInList);
                history.Add(state);
            }
        }
        else
        {
            state = rnd.Next(1, 10);
        }

        permaState = state;
       
    }

    public void skipTurn()
    {
        if (ready)
        {
            if(permaState == 8 || permaState == 9)
            {
                GameObject.Find("EventManager").GetComponent<EventManager>().hardClose();
            }
            quickStop();
            StartCoroutine(Wait3());
            
        }
    }

    public void startRoulette()
    {
        setWait();
        showEmpty();
        audioLever.PlayDelayed(1.2f);
        audioRolling.PlayDelayed(1.7f);
        randomState();
        m_Animator.SetTrigger("Open");
        Destroy(box.GetComponent("Billboard"));
        closing = false;

    }

    public void playRight()
    {
        audioRight.Play();
    }

    public void playWrong()
    {
        audioWrong.Play();
    }

    public void showRight(int type)
    {
        state = 99;
        left.GetComponent<Renderer>().material = Resources.Load("LeftRight") as Material;
        if (type == 0)
        {
            StartCoroutine(Wait());
        }
        if (type == 1)
        {
            ruota.GetComponent<Renderer>().material = Resources.Load("Ruota 10") as Material;
            StartCoroutine(Wait2());
        }
        if (type == 2)
        {
            ruota.GetComponent<Renderer>().material = Resources.Load("Ruota 11") as Material;
            StartCoroutine(Wait2());
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(10f);
        left.GetComponent<Renderer>().material = Resources.Load("Left") as Material;
    }

    private IEnumerator Wait2()
    {
        yield return new WaitForSeconds(2f);
        stopRoulette();
    }

    private IEnumerator Wait3()
    {
        yield return new WaitForSeconds(4f);
        startRoulette();
    }

    public void showWrong()
    {
        left.GetComponent<Renderer>().material = Resources.Load("LeftWrong") as Material;
        StartCoroutine(Wait());
    }

    public void showEmpty()
    {
        left.GetComponent<Renderer>().material = Resources.Load("Left") as Material;
    }

    public void setGo()
    {
        top.GetComponent<Renderer>().material = Resources.Load("TopGo") as Material;
        ready = true;
    }

    public void setWait()
    {
        top.GetComponent<Renderer>().material = Resources.Load("TopWait") as Material;
    }

    public void stopRoulette()
    {
        state = 0;
        ready = false;
        setWait();
        audioFireworks.PlayDelayed(1.3f);
        m_Animator.SetTrigger("Close");
        closing = true;
        rocket = GameObject.Find("Rocket");
        rocket.GetComponent<ParticleSystem>().Play();
        ruota.GetComponent<Renderer>().material = Resources.Load("Ruota") as Material;
    }

    public void quickStop()
    {
        state = 0;
        ready = false;
        setWait();
        m_Animator.SetTrigger("Close2");
        closing = true;
        ruota.GetComponent<Renderer>().material = Resources.Load("Ruota") as Material;
    }
}
