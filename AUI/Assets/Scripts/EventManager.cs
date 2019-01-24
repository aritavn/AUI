using HoloToolkit.UX.Progress;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    private static string TOUCH_SENSOR_HOLD = "4";
    private static string TOUCH_SENSOR_CUDDLE = "6";
    private static string RFID_SENSOR_FOOD = "e043781b";
    private static string RFID_SENSOR_SLEEP = "23a386d5";
    private static string RFID_SENSOR_ENTERTAINMENT = "45b41e39";
    private static string RFID_SENSOR_HYGIENE = "8f96dd00";


    private int cuddleCount = 0;
    private bool conditionVerified = false;
    private int lastRoueletteState = 0;
    private int rouletteState = 0;
    private Roulette roulette;

    private bool isTimerWorks = false;
    private bool isTouched1 = false;
    private bool isTouched2 = false;
    private float timeLeft = 5f;

    private void Start()
    {
        
    }
    private void Update()
    {
        
        if (isTimerWorks)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                ProgressIndicator.Instance.SetProgress(0f);
                isTimerWorks = false;
                timeLeft = 5f;
                isTouched1 = false;
                isTouched2 = false;
            }
        }

    }


    /**
     * @param currentEvent is the event received from the smart object
     * check if currentEvent is equal to the type of event that I expect
     */
    public void checkEvent(EventObject currentEvent)
    {

        lastRoueletteState = rouletteState;
        roulette = GameObject.Find("Box8").GetComponent<Roulette>();
        rouletteState = roulette.getState();

        int desiredEventType = defineCategoryForState(rouletteState);
        //int desiredEventType = 2;





        if (desiredEventType == 0)
        {
            // I don't have a desired event to check
            return;
        }

        if (currentEvent.getDuration() == null)
        {
            //if duration=0 it is the first event of the sensor
            return;
        }
        else
        {
            switch (desiredEventType)
            {
                case 1:
                    //hold the dolphin for 5 second
                    conditionVerified = checkHoldDolphin(currentEvent);
                    break;
                case 2:
                    //cuddle the dolphin 3 times

                    conditionVerified = checkCuddleDolphin(currentEvent);
                    break;

                case 3:
                    // RFID food
                    conditionVerified = checkRfidFood(currentEvent);
                    break;

                case 4:
                    //RFID hygiene
                    conditionVerified = checkRfidHygiene(currentEvent);
                    break;

                case 5:
                    //RFID entertainment
                    conditionVerified = checkRfidEnterteinment(currentEvent);
                    break;
                case 6:
                    //RFID sleep
                    conditionVerified = checkRfidSleep(currentEvent);
                    break;
            }

            if (conditionVerified)
            {
                Debug.Log("Condition verified");


                if (desiredEventType != 2 & desiredEventType != 1)
                {
                    roulette.showRight(0);//0
                    roulette.playRight();
                    //CALL TO MIRKO'S CODE
                    ChoiceManager choiceManager = GameObject.Find("ChoiceManager").GetComponent<ChoiceManager>();
                    choiceManager.GenerateObjectsInWorld(rouletteState);
                }
                else
                {
                    if (desiredEventType == 2)
                    {
                        roulette.showRight(2); //2
                        roulette.playRight();
                    }
                    if (desiredEventType == 1)
                    {
                        roulette.showRight(1);//1
                        roulette.playRight();
                    }

                    //roulette.stopRoulette();
                    StartCoroutine(Wait());
                }

            }
            else
            {
                //if (currentEvent.getID() == TOUCH_SENSOR_CUDDLE && desiredEventType == 2)
                if (cuddleCount != 0)
                {
                    roulette.playRight();
                }
                else
                {
                    Debug.Log("Condition not verified");
                    roulette.showWrong();
                    if(desiredEventType == 1 && (currentEvent.getID() == TOUCH_SENSOR_CUDDLE | currentEvent.getID() == TOUCH_SENSOR_HOLD) )
                    {
                        roulette.playRight();
                    }
                    else
                    {
                        roulette.playWrong();
                    }
                    if (desiredEventType == 2)
                    {
                        ProgressIndicator.Instance.SetProgress(0f);
                    }

                }

            }
        }



    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(10f);
        roulette.startRoulette();
    }
    private bool checkHoldDolphin(EventObject currentEvent)
    {
        if (currentEvent.getType() == "touch" &
           ((currentEvent.getID() == TOUCH_SENSOR_HOLD) | (currentEvent.getID() == TOUCH_SENSOR_CUDDLE)) &
           !isTimerWorks)
        {
            isTimerWorks = true;
            ProgressIndicator.Instance.SetProgress(0.5f);
            roulette.playRight();
            if (currentEvent.getID() == TOUCH_SENSOR_HOLD)
            {
                isTouched1 = true;
            }
            if (currentEvent.getID() == TOUCH_SENSOR_CUDDLE)
            {
                isTouched2 = true;
            }

        }
        else
        {
            if (currentEvent.getType() == "touch" &
           ((currentEvent.getID() == TOUCH_SENSOR_HOLD) | (currentEvent.getID() == TOUCH_SENSOR_CUDDLE)) &
           isTimerWorks)
            {
                if ((isTouched1 & currentEvent.getID() == TOUCH_SENSOR_CUDDLE) |
                    (isTouched2 & currentEvent.getID() == TOUCH_SENSOR_HOLD))
                {
                    ProgressIndicator.Instance.SetProgress(1f);
                    isTimerWorks = false;
                    timeLeft = 5f;
                    ProgressIndicator.Instance.Close();
                    return true;
                }


            }

        }

        return false;
    }

    private bool checkCuddleDolphin(EventObject currentEvent)
    {
        if (currentEvent.getType() == "touch" &
            currentEvent.getID() == TOUCH_SENSOR_CUDDLE)
        {

            cuddleCount++;
            Debug.Log("COUNT: " + cuddleCount);
            if (cuddleCount == 1)
            {
                ProgressIndicator.Instance.SetProgress(0.3f);
            }
            if (cuddleCount == 2)
            {
                ProgressIndicator.Instance.SetProgress(0.6f);
            }

            if (cuddleCount == 3)
            {
                ProgressIndicator.Instance.SetProgress(1f);
                resetCuddleCount();
                ProgressIndicator.Instance.Close();
                return true;
            }

        }
        else
        {
            resetCuddleCount();
        }
        return false;
    }

    private bool checkRfidFood(EventObject currentEvent)
    {
        if (currentEvent.getType() == "rfid" &
            currentEvent.getID() == RFID_SENSOR_FOOD)
        {
            return true;
        }
        return false;
    }

    private bool checkRfidHygiene(EventObject currentEvent)
    {
        if (currentEvent.getType() == "rfid" &&
            currentEvent.getID() == RFID_SENSOR_HYGIENE)
        {
            return true;
        }
        return false;
    }

    private bool checkRfidEnterteinment(EventObject currentEvent)
    {
        if (currentEvent.getType() == "rfid" &&
            currentEvent.getID() == RFID_SENSOR_ENTERTAINMENT)
        {
            return true;
        }
        return false;
    }

    private bool checkRfidSleep(EventObject currentEvent)
    {
        if (currentEvent.getType() == "rfid" &&
            currentEvent.getID() == RFID_SENSOR_SLEEP)
        {
            return true;
        }
        return false;
    }


    private int defineCategoryForState(int rouletteState)
    {
        int category = 0;
        switch (rouletteState)
        {
            case 1:
                category = 3;
                break;
            case 2:
                category = 4;
                break;
            case 3:
                category = 4;
                break;
            case 4:
                category = 6;
                break;
            case 5:
                category = 3;
                break;
            case 6:
                category = 5;
                break;
            case 7:
                category = 5;
                break;
            case 8:
                category = 1;
                break;
            case 9:
                category = 2;
                break;
            case 10:
                category = 1;
                break;
            case 11:
                category = 2;
                break;
        }

        return category;
    }


    public int getCuddleCount()
    {
        return cuddleCount;
    }

    public void resetCuddleCount()
    {
        cuddleCount = 0;
    }
}
