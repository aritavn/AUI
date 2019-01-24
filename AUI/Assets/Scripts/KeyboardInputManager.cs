using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        //CUDDLE
        if (Input.GetKeyDown(KeyCode.Z))
        {
            
            Debug.Log("CUDDLE");
            string jsonString = "{\"events\": [{\"typ\": \"touch\", \"val\": \"6\", \"act\": 1, \"dur\": \"1000\"}]}";

            EventObject eventObjcet = JsonParser.parse(jsonString);
            EventManager eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
            eventManager.checkEvent(eventObjcet);
        }

        //RFID FOOD
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("RFID FOOD");
            string jsonString = "{\"events\": [{\"typ\": \"rfid\", \"val\": \"e043781b\", \"act\": 1, \"dur\": \"1000\"}]}";

            EventObject eventObjcet = JsonParser.parse(jsonString);
            EventManager eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
            eventManager.checkEvent(eventObjcet);
        }

        //RFID HYGIENE
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("RFID HYGIENE");
            string jsonString = "{\"events\": [{\"typ\": \"rfid\", \"val\": \"8f96dd00\", \"act\": 1, \"dur\": \"1000\"}]}";

            EventObject eventObjcet = JsonParser.parse(jsonString);
            EventManager eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
            eventManager.checkEvent(eventObjcet);
        }

        //RFID ENTERTAINMENT
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("RFID ENTERTAINMENT");
            string jsonString = "{\"events\": [{\"typ\": \"rfid\", \"val\": \"45b41e39\", \"act\": 1, \"dur\": \"1000\"}]}";

            EventObject eventObjcet = JsonParser.parse(jsonString);
            EventManager eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
            eventManager.checkEvent(eventObjcet);
        }

        //RFID SLEEP
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("RFID SLEEP");
            string jsonString = "{\"events\": [{\"typ\": \"rfid\", \"val\": \"23a386d5\", \"act\": 1, \"dur\": \"1000\"}]}";

            EventObject eventObjcet = JsonParser.parse(jsonString);
            EventManager eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
            eventManager.checkEvent(eventObjcet);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("HOLD");
            string jsonString = "{\"events\": [{\"typ\": \"touch\", \"val\": \"4\", \"act\": 1, \"dur\": \"5000\"}]}";

            EventObject eventObjcet = JsonParser.parse(jsonString);
            EventManager eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
            eventManager.checkEvent(eventObjcet);

        }

    }
}
