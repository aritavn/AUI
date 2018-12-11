using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{

    private static string TOUCH_SENSOR_HOLD = "5";
    private static string TOUCH_SENSOR_CUDDLE = "2";
    private static string RFID_SENSOR_FOOD = "7";
    private static string RFID_SENSOR_SLEEP = "8";
    private static string RFID_SENSOR_ENTERTAINMENT = "9";
    private static string RFID_SENSOR_HYGIENE = "10";


    private int cuddleCount;
    private bool conditionVerified = false;
    private int lastRoueletteState = 0;
    private int rouletteState = 0;

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

        lastRoueletteState = rouletteState;
        Roulette roulette = GameObject.Find("Box8").GetComponent<Roulette>();
        rouletteState = roulette.getState();

        //RouletteRandom roulette = GameObject.Find("Roulette").GetComponent<RouletteRandom>();
        

        Debug.Log("roulette state: " + rouletteState + " last state: " + lastRoueletteState);
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

        if (currentEvent.getDuration() == 0)
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
                //CALL TO MIRKO'S CODE
                //GenerateObjectsInWorld(rouletteState);
            }
        }



    }

    private bool checkHoldDolphin(EventObject currentEvent)
    {
        if (currentEvent.getType() == "touch" &
           currentEvent.getID() == TOUCH_SENSOR_HOLD &
           currentEvent.getDuration() >= 5000)
        {
            return true;
        }

        return false;
    }

    private bool checkCuddleDolphin(EventObject currentEvent)
    {
        if (currentEvent.getType() == "touch" &
            currentEvent.getID() == TOUCH_SENSOR_CUDDLE)
        {
            Debug.Log("CUDDLE");
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
