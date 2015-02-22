/*
 * Copyright (c) 2015 Katusyohi Hotta  
 */

namespace UnityOSC
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Collections;
    using System.Collections.Generic;

    public class OscMessage
    {
        private string adress;

        private ArrayList MainBuffer;
        private ArrayList Bundle;
        private ArrayList TimeTag;
        private ArrayList DataSize;
        private ArrayList Adress;
        private ArrayList TypeList;
        private ArrayList BufferData;

        private byte[] pad = { 0 };
        byte[] dot = { 44 }; //","
        byte[] pads = { 0, 0, 0, 0 };

        public OscMessage()
        {
            init();
        }

        private void init()
        {
            adress = "/Unity/";

            MainBuffer = new ArrayList();
            Bundle = new ArrayList();
            TimeTag = new ArrayList();
            DataSize = new ArrayList();
            Adress = new ArrayList();
            TypeList = new ArrayList(dot);
            BufferData = new ArrayList();
        }

        public byte[] Encoder()
        {
            byte[] data;
            
            //Header
            data = Encoding.ASCII.GetBytes("#bundle");
            this.Bundle.AddRange(data);

            //OSC Time tag
            byte[] time = { 0, 0, 0, 0, 0, 0, 0, 0, 1 };
            this.TimeTag.AddRange(time);

            //Adress part
            data = Encoding.ASCII.GetBytes(this.adress);
            this.Adress.AddRange(data);
			int Adress_Padding = this.Adress.Count % 4;
			if (Adress_Padding == 0) 
			{
				this.Adress.AddRange (pad);
			} 
			else 
			{
				Adress_Padding  = 4 - Adress_Padding ;
				for (var i = 0; i < Adress_Padding ; i++)
				{
					this.Adress.AddRange(pad);
				}
			}

            //TypeList
            int TypeList_Padding = this.TypeList.Count % 4;
            if (TypeList_Padding == 0)
            {
                this.TypeList.AddRange(pad);
            }
            else
            {
                TypeList_Padding = 4 - TypeList_Padding;
                for (var i = 0; i < TypeList_Padding; i++)
                {
                    this.TypeList.AddRange(pad);
                }
            }

            //DataPart Size (adress size + type size + data size)
            int size = (this.Adress.Count + this.TypeList.Count + this.BufferData.Count);
            data = BitConverter.GetBytes(size);
            if (BitConverter.IsLittleEndian)
                data = SwapEndian(data);
            this.DataSize.AddRange(data);

            //Making MainBuffer
            this.MainBuffer.AddRange(Bundle);
            this.MainBuffer.AddRange(TimeTag);
            this.MainBuffer.AddRange(DataSize);
            this.MainBuffer.AddRange(Adress);
            this.MainBuffer.AddRange(TypeList);

            this.MainBuffer.AddRange(BufferData);

            return (byte[])MainBuffer.ToArray(typeof(byte));
        }

        private byte[] SwapEndian(byte[] data)
        {
            byte[] swapped = new byte[data.Length];
            for (int i = data.Length - 1, j = 0; i >= 0; i--, j++)
            {
                swapped[j] = data[i];
            }
            return swapped;
        }

        public void setAdress(string adress)
        {
            this.adress = adress;
        }

        public void addFloatArg(float argv)
        {
            byte[] type, data;

            type = Encoding.ASCII.GetBytes("f");
            TypeList.AddRange(type);

            data = BitConverter.GetBytes(argv);
            if (BitConverter.IsLittleEndian)
                data = SwapEndian(data);
            BufferData.AddRange(data);
        }

        public void addInt64Arg(Int64 argv)
        {
            byte[] type, data;

            type = Encoding.ASCII.GetBytes("h");
            TypeList.AddRange(type);

            data = BitConverter.GetBytes(argv);
            if (BitConverter.IsLittleEndian)
                data = SwapEndian(data);
            BufferData.AddRange(data);
        }

        public void addIntArg(Int32 argv)
        {
            byte[] type, data;

            type = Encoding.ASCII.GetBytes("i");
            TypeList.AddRange(type);

            data = BitConverter.GetBytes(argv);
            if (BitConverter.IsLittleEndian)
                data = SwapEndian(data);
            BufferData.AddRange(data);
        }

        public void addStringArg(string argv)
        {
            byte[] type, data;
            int size;

            type = Encoding.ASCII.GetBytes("s");
            TypeList.AddRange(type);

            data = Encoding.ASCII.GetBytes(argv);
            BufferData.AddRange(data);

            size = data.Length % 4;
            if (size == 0)
            {
                for (var i = 0; i < 4; i++)
                {
                    BufferData.AddRange(pad);
                }
            }
            else
            {
                size = 4 - size;
                for (var i = 0; i < size; i++)
                {
                    BufferData.AddRange(pad);
                }
            }
        }

        public void addBlobArg(byte[] argv)
        {
            byte[] type, size;

            type = Encoding.ASCII.GetBytes("b");
            TypeList.AddRange(type);

            size = BitConverter.GetBytes(argv.Length);
            if (BitConverter.IsLittleEndian)
                size = SwapEndian(size);
            BufferData.AddRange(size);

            BufferData.AddRange(argv);
        }

        public void clear()
        {
            init();
        }
    }

    //keijiroさんの"Unity-osc"を参考
    class OscPacket
    {
        private static Message msg;

        private byte[] StreamBuffer;
        private int readPoint;

        public OscPacket()
        {
            msg = new Message();
            readPoint = 0;
        }

        public Message Decoder(byte[] buffer)
        {
            msg.BufferSize = buffer.Length;

            this.StreamBuffer = buffer;
            this.readPoint = 0;

            msg.path = ReadString();

            if (msg.path == "#bundle")
            {
                msg.time     = ReadInt64();
                msg.DataSize = ReadInt32();
                msg.adress   = ReadString();
                msg.types    = ReadString();

                msg.data = new List<object>();

                for (var i = 0; i < msg.types.Length - 1; i++)
                {
                    switch (msg.types[i + 1])
                    {
                        case 'i':
                            msg.data.Add(ReadInt32());
                            break;
                        case 'h':
                            msg.data.Add(ReadInt64());
                            break;
                        case 'f':
                            msg.data.Add(ReadFloat32());
                            break;
                        case 's':
                            msg.data.Add(ReadString());
                            break;
                        case 'b':
                            msg.data.Add(ReadBlob());
                            break;
                    }
                }
            }

            return msg;
        }

        private float ReadFloat32()
        {
            Byte[] temp = {
                StreamBuffer[readPoint + 3],
                StreamBuffer[readPoint + 2],
                StreamBuffer[readPoint + 1],
                StreamBuffer[readPoint]
            };
            readPoint += 4;
            return BitConverter.ToSingle(temp, 0);
        }

        private int ReadInt32()
        {
            int temp =
                (StreamBuffer[readPoint + 0] << 24) +
                (StreamBuffer[readPoint + 1] << 16) +
                (StreamBuffer[readPoint + 2] << 8) +
                (StreamBuffer[readPoint + 3]);
            readPoint += 4;
            return temp;
        }

        private long ReadInt64()
        {
            long temp =
                ((long)StreamBuffer[readPoint + 0] << 56) +
                ((long)StreamBuffer[readPoint + 1] << 48) +
                ((long)StreamBuffer[readPoint + 2] << 40) +
                ((long)StreamBuffer[readPoint + 3] << 32) +
                ((long)StreamBuffer[readPoint + 4] << 24) +
                ((long)StreamBuffer[readPoint + 5] << 16) +
                ((long)StreamBuffer[readPoint + 6] << 8) +
                ((long)StreamBuffer[readPoint + 7]);
            readPoint += 8;
            return temp;
        }

        private string ReadString()
        {
            var offset = 0;
            while (StreamBuffer[readPoint + offset] != 0)
            {
                offset++;
            }
            var s = System.Text.Encoding.UTF8.GetString(StreamBuffer, readPoint, offset);
            readPoint += (offset + 4) & ~3;
            return s;
        }

        private Byte[] ReadBlob()
        {
            var length = ReadInt32();
            var temp = new Byte[length];
            Array.Copy(StreamBuffer, readPoint, temp, 0, length);
            readPoint += (length + 3) & ~3;
            return temp;
        }
    }

    public struct Message
    {
        public string path;
        public string adress;
        public long time;
        public int BufferSize;
        public int DataSize;
        public string types;
        public List<object> data;
    }

    public class OscSocket
    {
        //UDP Socket
        private UdpClient udpClient;
        private IPEndPoint endPoint, remotePoint;

        //Osc Packet
        private OscPacket oscpacket;

        //Event
        public delegate void resive(IPEndPoint RemotePint, Message Message);
        public event resive OnMassage;
        private Thread Recive_thread;

        //OscSocket Opetions
        public string host;
        public int RemotePort;
        public int ListenPort;

        public void Setup(string host, int RemotePort, int ListenPort)
        {
            this.host = host;
            this.RemotePort = RemotePort;
            this.ListenPort = ListenPort;

            endPoint = new IPEndPoint(IPAddress.Parse(host), RemotePort);
            udpClient = new UdpClient(ListenPort);

            oscpacket = new OscPacket();

            Recive_thread = new Thread(new ThreadStart(stream));
            Recive_thread.Start();
        }

        public void send(OscMessage msg)
        {
            byte[] buffer;
            buffer = msg.Encoder();

            UnityEngine.Debug.Log(buffer.Length);

            udpClient.Send(buffer, buffer.Length, endPoint);
        }

        public void close()
        {
            Recive_thread.Abort();
            udpClient.Close();
        }

        private void stream()
        {
            while (true)
            {
                Thread.Sleep(0);
                while (udpClient.Available > 0)
                {
                    Message msg = oscpacket.Decoder(udpClient.Receive(ref remotePoint));
                    OnMassage(remotePoint, msg);
                }
            }
        }
    }
}