using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{

    private static string TOUCH_SENSOR_HOLD = "6";
    private static string TOUCH_SENSOR_CUDDLE = "4";
    private static string RFID_SENSOR_FOOD = "e043781b";
    private static string RFID_SENSOR_SLEEP = "23a386d5";
    private static string RFID_SENSOR_ENTERTAINMENT = "45b41e39";
    private static string RFID_SENSOR_HYGIENE = "45b41e39";


    private int cuddleCount;
    private bool conditionVerified = false;
    private int lastRoueletteState = 0;
    private int rouletteState = 0;
    private Roulette roulette;

    private static EventManager instance = null;

    private EventManager()
    {

    }

    static public EventManager getEventManager()
    {
        if (instance == null)
        {
            instance = new EventManager();
        }

        return instance;
    }


    /**
     * @param currentEvent is the event received from the smart object
     * check if currentEvent is equal to the type of event that I expect
     */
    public void checkEvent(EventObject currentEvent)
    {
        Debug.Log("Check ID interno: "+currentEvent.getID());
        lastRoueletteState = rouletteState;
        roulette = GameObject.Find("Box8").GetComponent<Roulette>();
        rouletteState = roulette.getState();
        

        //Debug.Log("roulette state: " + rouletteState + " last state: " + lastRoueletteState);
        if (rouletteState == lastRoueletteState && conditionVerified == true)
        {
            return;
        }

        if (lastRoueletteState != 11 && rouletteState == 11 && cuddleCount != 0)
        {
            resetCuddleCount();
        }
        /*if (lastRoueletteState == 11 && cuddleCount != 0 && rouletteState != lastRoueletteState)
        {
            resetCuddleCount();
        }*/

        int desiredEventType = defineCategoryForState(rouletteState);





        if (desiredEventType == 0)
        {
            // I don't have a desired event to check
            return;
        }

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
                roulette.showRight();
                roulette.playRight();

                //CALL TO MIRKO'S CODE
                ChoiceManager choiceManager = GameObject.Find("ChoiceManager").GetComponent<ChoiceManager>();
                choiceManager.GenerateObjectsInWorld(rouletteState);
            }
            else
            {
                Debug.Log("Condition not verified");
                roulette.showWrong();
                roulette.playWrong();
            }
        



    }

    private bool checkHoldDolphin(EventObject currentEvent)
    {
        Debug.Log("CHECK HOLD");
        if (currentEvent.getType() == "touch" &
           currentEvent.getID() == TOUCH_SENSOR_HOLD &
           int.Parse(currentEvent.getDuration()) >= 5000)
        {
            return true;
        }

        return false;
    }

    private bool checkCuddleDolphin(EventObject currentEvent)
    {
        Debug.Log("CHECK CUDDLE");
        if (currentEvent.getType() == "touch" &
            currentEvent.getID() == TOUCH_SENSOR_CUDDLE)
        {
            Debug.Log("CHECK CUDDLE");
            cuddleCount++;
            if (cuddleCount == 3)
            {
                resetCuddleCount();
                return true;
            }
        }
        return false;
    }

    private bool checkRfidFood(EventObject currentEvent)
    {
        Debug.Log("CHECK FOOD");
        if (currentEvent.getType() == "rfid" &
            currentEvent.getID() == RFID_SENSOR_FOOD)
        {
            return true;
        }
        return false;
    }

    private bool checkRfidHygiene(EventObject currentEvent)
    {
        Debug.Log("CHECK HYGIENE");
        if (currentEvent.getType() == "rfid" &&
            currentEvent.getID() == RFID_SENSOR_HYGIENE)
        {
            return true;
        }
        return false;
    }

    private bool checkRfidEnterteinment(EventObject currentEvent)
    {
        Debug.Log("CHECK ENTERTEINMENT");
        if (currentEvent.getType() == "rfid" &&
            currentEvent.getID() == RFID_SENSOR_ENTERTAINMENT)
        {
            return true;
        }
        return false;
    }

    private bool checkRfidSleep(EventObject currentEvent)
    {
        Debug.Log("CHECK SLEEP");
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
