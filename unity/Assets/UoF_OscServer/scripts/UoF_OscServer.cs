using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class UoF_OscServer : MonoBehaviour
{
    public int listenPort = 8080;
    UdpClient udpClient;
    IPEndPoint endPoint;
    Osc.Parser osc = new Osc.Parser ();

    private bool getTexture = true;
    public Texture2D img;
    public int width,height,size;

    public bool ready = false;

    public Material mate;

    void Start ()
    {
        endPoint = new IPEndPoint (IPAddress.Any, listenPort);
        udpClient = new UdpClient (endPoint);
    }

    void Update ()
    {
        while (udpClient.Available > 0) {
            osc.FeedData (udpClient.Receive (ref endPoint));
        }

        while (osc.MessageCount > 0) {
            var msg = osc.PopMessage ();

            if (getTexture)
            {
                getTexture = false;
                img = new Texture2D(width, height);
            }

            width  = (int)msg.data[0];
            height = (int)msg.data[1];
            size   = (int)msg.data[2];
            img.LoadImage((byte[])msg.data[3]);

            mate.mainTexture = img;
            ready = true;
        }
    }
}