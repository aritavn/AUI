
using Newtonsoft.Json.Linq;

public class JsonParser
{
    private string jsonString;


	public JsonParser()
	{
	}

    public static EventObject parse(string jsonString)
    {


        JObject jsonObjcet = JObject.Parse(jsonString);
        JArray jArrayEvents= (JArray)jsonObjcet["events"];
        EventObject eventObject = null ;

        // if arrive more events in a unique call
        //List<EventObject> eventObjectsList = new List<EventObject>();

        foreach (JObject jObject in jArrayEvents)
        {
            string type = (string)jObject.SelectToken("typ");
            string id = (string)jObject.SelectToken("val");
            int active = (int)jObject.SelectToken("act");
            if (jObject.SelectToken("dur") != null)
            {
                int duration = (int)jObject.SelectToken("dur");
                eventObject = new EventObject(type, id, active, duration);
            } else
            {
                eventObject = new EventObject(type, id, active, 0);
            }
            
        }
        return eventObject;
    }



}
