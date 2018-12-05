using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette : MonoBehaviour {
    private GameObject ruota;
    private GameObject box;
    private Animator m_Animator;
    private GameObject rocket;
    private bool closing = false;

    private int state;
	// Use this for initialization
	void Start () {
        ruota = GameObject.Find("pCylinder7");
        box = GameObject.Find("Box8");
        m_Animator = box.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Open") &&
            m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f &&
            closing==false)
        {
            randomState();
            switch (state)
            {
                case 2:
                    ruota.GetComponent<Renderer>().material = Resources.Load("Ruota 3") as Material;
                    break;
                default:
                    break;
            }
        }
    }

    public int getState()
    {
        return state;
    }

    private void randomState()
    {
        System.Random rnd = new System.Random();
        state = rnd.Next(1, 10);
        state = 2;
    }

    public void startRoulette()
    {
        m_Animator.SetTrigger("Open");
        Destroy(box.GetComponent("Billboard"));
        closing = false;

    }

    public void stopRoulette()
    {
        m_Animator.SetTrigger("Close");
        closing = true;
        rocket = GameObject.Find("Rocket");
        rocket.GetComponent<ParticleSystem>().Play();
        ruota.GetComponent<Renderer>().material = Resources.Load("Ruota") as Material;
    }
}
