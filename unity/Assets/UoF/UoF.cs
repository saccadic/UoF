using UnityEngine;

namespace Unityof
{
    using WebSocketSharp;
    using System.Collections;

    public class UoF : MonoBehaviour
    {

        public Texture2D texture;

        public void send(WebSocket ws, string massage)
        {
            if (ws.IsAlive)
            {
                ws.Send(massage);
            }
        }

        public void disconnect(WebSocket ws)
        {
            if (ws.IsAlive)
            {
                ws.Close();
            }
        }

        public Texture2D loadtexture(byte[] image, long width, long height)
        {
            texture = new Texture2D((int)width, (int)height);
            texture.LoadImage(image);

            return texture;
        }
    }
}



