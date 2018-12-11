using System;

public class EventObject
{
    string type;
    string id;
    int active;
    int duration;
    public EventObject(string type, string id, int active, int duration)
	{
        this.type = type;
        this.id = id;
        this.active = active;
        this.duration = duration;

	}

    public string getType()
    {
        return type;
    }

    public int getDuration()
    {
        return duration;
    }

    public string getID()
    {
        return id;
    }

    public int getActive()
    {
        return active;
    }
}
