using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if !UNITY_EDITOR
    using Windows.Networking;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;
#endif

//Able to act as a reciever 
public class Server : MonoBehaviour, IDisposable
{
    public String _input = "Waiting";
    ArrayList lista = new ArrayList();
#if !UNITY_EDITOR
        StreamSocket socket;
        StreamSocketListener listener;
        String port;
        String message;
        
#endif

    // Use this for initialization
    void Start()
    {

#if !UNITY_EDITOR
        listener = new StreamSocketListener();
        port = "7007";
        listener.ConnectionReceived += Listener_ConnectionReceived;
        listener.Control.KeepAlive = false;

        Listener_Start();
#endif
    }

#if !UNITY_EDITOR
    private async void Listener_Start()
    {
        Debug.Log("Listener started");
        print("Listener started");
        try
        {
            await listener.BindServiceNameAsync(port);
            }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }

        Debug.Log("Listening");
    }

    private async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
    {
        Debug.Log("Connection received");

        try
        {
                    using (var dr = new DataReader(args.Socket.InputStream))
                    {
                        dr.InputStreamOptions = InputStreamOptions.Partial;
                        await dr.LoadAsync(500);

                        int check = 0;
                        int par=0;
                        string message = "";        

                        while (true)
                        {

                            var input = dr.ReadString(1);
                            if (input == "{")
                                {  
                                    par++;
                                    check = 1;
                                }

                            if (check == 1)
                            {   
                                    message += input; 
                            }
                            
                            if (input == "}")
                                {
                                    par--;
                                    if(par==0)
                                        {break;}
                                }
                        }
                        string nuova = "";
                         char[] ch = message.ToCharArray();
                        for(int i=0; i<ch.Length; i++)
                        {
                            if (ch[i] != '\\')
                                {
                                     nuova = nuova + ch[i].ToString();
                                }
                        }
                Debug.Log("IL NOSTRO MESSAGGIO:  " + nuova);
                //Debug.Log("IL NOSTRO MESSAGGIO:  " + message);
                lista.Add(nuova);
                        
                        
                        
                        //Debug.Log("received: " + input);
                        //_input = input;

                    }  
        }
        catch (Exception e)
        {
            Debug.Log("disconnected!!!!!!!! " + e);
        }
         finally
            {
                //Dispose();
                args.Socket.Dispose();   //This call is important: This closes the connection to the client.
            }

    }

#endif
    public void Dispose()
    {
#if !UNITY_EDITOR
            if (listener != null)
            {
                listener.ConnectionReceived -= Listener_ConnectionReceived;
                listener.Dispose();
                listener = null;
            }
#endif
    }
    void Update()
    {
       if(lista.Count!=0)
        {
            string mex = lista[0].ToString();
            EventObject eventObjcet = JsonParser.parse(mex);
            EventManager eventManager = EventManager.getEventManager();
            Debug.Log("TYPE: "+eventObjcet.getType()+" ID evento:"+eventObjcet.getID()+ " ACT: "+eventObjcet.getActive()+ " DURATION: "+ eventObjcet.getDuration());
            if(eventObjcet.getID() != "")
            {
                eventManager.checkEvent(eventObjcet);
            }
            
            lista.RemoveAt(0);
        }
        
    }
}