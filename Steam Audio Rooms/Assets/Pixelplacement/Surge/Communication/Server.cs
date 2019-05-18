/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
///
/// A super simple server for sending data back and forth.
/// 
/// NOTE: Communication does not support Unity's new networking system introduced in Unity 2019 - a new version will be created soon.
///
/// </summary>

#if !UNITY_2019

#pragma warning disable 0618

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;
using System.Net;

namespace Pixelplacement
{
    public class Server : NetworkDiscovery
    {
        //Private Classes:
        private class ServerInstance : NetworkServerSimple
        {
            //Public Events:
            public event Action<NetworkConnection> OnConnection;
            public event Action<NetworkConnection> OnDisconnection;

            //Overrides:
            public override void OnConnected(NetworkConnection conn)
            {
                base.OnConnected(conn);
                if (OnConnection != null) OnConnection(conn);
            }

            public override void OnDisconnected(NetworkConnection conn)
            {
                base.OnDisconnected(conn);
                if (OnDisconnection != null) OnDisconnection(conn);
            }
        }

        //Public Events:
        public static event Action<string> OnPlayerConnected;
        public static event Action<string> OnPlayerDisconnected;
        public static event Action<FloatMessage> OnFloat;
        public static event Action<FloatArrayMessage> OnFloatArray;
        public static event Action<IntMessage> OnInt;
        public static event Action<IntArrayMessage> OnIntArray;
        public static event Action<Vector2Message> OnVector2;
        public static event Action<Vector2ArrayMessage> OnVector2Array;
        public static event Action<Vector3Message> OnVector3;
        public static event Action<Vector3ArrayMessage> OnVector3Array;
        public static event Action<QuaternionMessage> OnQuaternion;
        public static event Action<QuaternionArrayMessage> OnQuaternionArray;
        public static event Action<Vector4Message> OnVector4;
        public static event Action<Vector4ArrayMessage> OnVector4Array;
        public static event Action<RectMessage> OnRect;
        public static event Action<RectArrayMessage> OnRectArray;
        public static event Action<StringMessage> OnString;
        public static event Action<StringArrayMessage> OnStringArray;
        public static event Action<ByteMessage> OnByte;
        public static event Action<ByteArrayMessage> OnByteArray;
        public static event Action<ColorMessage> OnColor;
        public static event Action<ColorArrayMessage> OnColorArray;
        public static event Action<Color32Message> OnColor32;
        public static event Action<Color32ArrayMessage> OnColor32Array;
        public static event Action<BoolMessage> OnBool;
        public static event Action<BoolArrayMessage> OnBoolArray;
        public static event Action<Matrix4x4Message> OnMatrix4x4;
        public static event Action<Matrix4x4ArrayMessage> OnMatrix4x4Array;

        //Public Variables:
        [Tooltip("Must match the client's primary quality of service.")] public QosType primaryQualityOfService = QosType.Reliable;
        [Tooltip("Must match the client's secondary quality of service.")] public QosType secondaryQualityOfService = QosType.Unreliable;
        [Tooltip("Optional name for this device to be sent to clients for connection identification. Note: this cannot be changed after application begins.")] public string customDeviceId;
        [Tooltip("Must match the client's broadcasting port.")] public int broadcastingPort;
        public int maxConnections;
        public uint initialBandwidth;

        //Private Variables:
        private static Dictionary<int, string> _connections = new Dictionary<int, string>();
        private static ServerInstance _server = new ServerInstance();
        private static string _randomIdKey = "RandomIdKey";
        private static string ipAddress;

        //Public Properties:
        public static int ConnectedCount
        {
            get
            {
                return _connections.Count;
            }
        }

        public static int PrimaryChannel
        {
            get;
            private set;
        }

        public static int SecondaryChannel
        {
            get;
            private set;
        }

        public static string DeviceId
        {
            get
            {
                GenerateID();
                return PlayerPrefs.GetString(_randomIdKey);
            }
        }

        public static bool Running
        {
            get;
            private set;
        }

        public static bool Connected
        {
            get
            {
                return ConnectedCount > 0;
            }
        }

        //Private Properties:
        private int ServerPort
        {
            get
            {
                return base.broadcastPort + 1;
            }
        }

        //Init:
        public void Reset()
        {
            maxConnections = 1;
            showGUI = false;
            broadcastingPort = 47777;
            initialBandwidth = 500000;
            primaryQualityOfService = QosType.Reliable;
            secondaryQualityOfService = QosType.Unreliable;
        }

        private void Start()
        {
            //get ip address since we aren't using NetworkManager and cannot use that singleton:
            ipAddress = "::ffff:" + IPManager.GetIP(ADDRESSFAM.IPv4);

            //setup:
            broadcastPort = broadcastingPort;

            //set device id:
            broadcastData = ServerPort.ToString();
            if (string.IsNullOrEmpty(customDeviceId))
            {
                broadcastData += "_" + DeviceId;
            }
            else
            {
                broadcastData += "_" + customDeviceId;
            }

            //HACK: this is a fix for the broadcastData bug where Unity will combine different 
            //data if the length is different because they internally reuse this object
            broadcastData += "~!~";

            Init();

            //configurations:
            ConnectionConfig config = new ConnectionConfig();
            PrimaryChannel = config.AddChannel(primaryQualityOfService);
            SecondaryChannel = config.AddChannel(secondaryQualityOfService);
            config.InitialBandwidth = initialBandwidth;

            HostTopology topology = new HostTopology(config, maxConnections);
            _server.Listen(ServerPort, topology);

            //event hooks:
            _server.OnConnection += HandleConnect;
            _server.OnDisconnection += HandleDisconnect;
            _server.RegisterHandler((short)NetworkMsg.FloatMsg, HandleFloat);
            _server.RegisterHandler((short)NetworkMsg.FloatArrayMsg, HandleFloatArray);
            _server.RegisterHandler((short)NetworkMsg.IntMsg, HandleInt);
            _server.RegisterHandler((short)NetworkMsg.IntArrayMsg, HandleIntArray);
            _server.RegisterHandler((short)NetworkMsg.Vector2Msg, HandleVector2);
            _server.RegisterHandler((short)NetworkMsg.Vector2ArrayMsg, HandleVector2Array);
            _server.RegisterHandler((short)NetworkMsg.Vector3Msg, HandleVector3);
            _server.RegisterHandler((short)NetworkMsg.Vector3ArrayMsg, HandleVector3Array);
            _server.RegisterHandler((short)NetworkMsg.QuaternionMsg, HandleQuaternion);
            _server.RegisterHandler((short)NetworkMsg.QuaternionArrayMsg, HandleQuaternionArray);
            _server.RegisterHandler((short)NetworkMsg.Vector4Msg, HandleVector4);
            _server.RegisterHandler((short)NetworkMsg.Vector4ArrayMsg, HandleVector4Array);
            _server.RegisterHandler((short)NetworkMsg.RectMsg, HandleRect);
            _server.RegisterHandler((short)NetworkMsg.RectArrayMsg, HandleRectArray);
            _server.RegisterHandler((short)NetworkMsg.StringMsg, HandleString);
            _server.RegisterHandler((short)NetworkMsg.StringArrayMsg, HandleStringArray);
            _server.RegisterHandler((short)NetworkMsg.ByteMsg, HandleByte);
            _server.RegisterHandler((short)NetworkMsg.ByteArrayMsg, HandleByteArray);
            _server.RegisterHandler((short)NetworkMsg.ColorMsg, HandleColor);
            _server.RegisterHandler((short)NetworkMsg.ColorArrayMsg, HandleColorArray);
            _server.RegisterHandler((short)NetworkMsg.Color32Msg, HandleColor32);
            _server.RegisterHandler((short)NetworkMsg.Color32ArrayMsg, HandleColor32Array);
            _server.RegisterHandler((short)NetworkMsg.BoolMsg, HandleBool);
            _server.RegisterHandler((short)NetworkMsg.BoolArrayMsg, HandleBoolArray);
            _server.RegisterHandler((short)NetworkMsg.Matrix4x4Msg, HandleMatrix4x4);
            _server.RegisterHandler((short)NetworkMsg.Matrix4x4ArrayMsg, HandleMatrix4x4Array);

            //dont destroy:
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }

        //Cleanup:
        private void OnDestroy()
        {
            _server.Stop();
        }

        //Flow
        private void OnEnable()
        {
            Running = true;
        }

        private void OnDisable()
        {
            Running = false;
        }

        //Private Methods:
        private void Init()
        {
            transform.parent = null;
            Initialize();
            StartAsServer();
        }

        private static void GenerateID()
        {
            //key already available:
            if (PlayerPrefs.HasKey(_randomIdKey)) return;

            string id = "";

            //create guid:
            Guid guid = Guid.NewGuid();
            string guidString = guid.ToString();

            //splint into strings:
            string[] parts = guidString.Split('-');

            //go through each block in the guid to create a 5 digit id:
            for (int i = 0; i < parts.Length; i++)
            {
                //holder for each int found:
                int finalNumber = 0;

                foreach (var item in parts[i])
                {
                    //if it's a number then add it:
                    int currentNumber = 0;
                    if (int.TryParse(item.ToString(), out currentNumber))
                    {
                        finalNumber += currentNumber;
                    }
                }

                //loop number so it is a single digit:
                finalNumber = (int)Mathf.Repeat(finalNumber, 9);

                //construct string:
                id += finalNumber.ToString();
            }

            PlayerPrefs.SetString(_randomIdKey, id);
        }

        //Public Methods:
        public static void RegisterHandler(short msgType, NetworkMessageDelegate handler)
        {
            _server.RegisterHandler(msgType, handler);
        }

        public static void UnregisterHandler(short msgType)
        {
            _server.UnregisterHandler(msgType);
        }

        public static void Disconnect()
        {
            _server.DisconnectAllConnections();
        }

        public static void Send(short msgType, MessageBase message, string address = "", int qualityOfServiceChannel = 0)
        {
            foreach (var item in _server.connections)
            {
                if (item != null)
                {
                    //provided an address?
                    if (address != "")
                    {
                        if (address != item.address)
                        {
                            continue;
                        }
                    }

                    item.SendByChannel(msgType, message, qualityOfServiceChannel);
                }
            }
        }

        public static void Send(Matrix4x4 value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.Matrix4x4Msg, new Matrix4x4Message(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(Matrix4x4[] value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.Matrix4x4ArrayMsg, new Matrix4x4ArrayMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(float value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.FloatMsg, new FloatMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(float[] value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.FloatArrayMsg, new FloatArrayMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(int value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.IntMsg, new IntMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(int[] value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.IntArrayMsg, new IntArrayMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(Vector2 value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.Vector2Msg, new Vector2Message(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(Vector2[] value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.Vector2ArrayMsg, new Vector2ArrayMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(Vector3 value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.Vector3Msg, new Vector3Message(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(Vector3[] value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.Vector3ArrayMsg, new Vector3ArrayMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(Quaternion value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.QuaternionMsg, new QuaternionMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(Quaternion[] value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.QuaternionArrayMsg, new QuaternionArrayMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(Vector4 value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.Vector4Msg, new Vector4Message(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(Vector4[] value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.Vector4ArrayMsg, new Vector4ArrayMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(Rect value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.RectMsg, new RectMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(Rect[] value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.RectArrayMsg, new RectArrayMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(string value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.StringMsg, new StringMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(string[] value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.StringArrayMsg, new StringArrayMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(byte value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.ByteMsg, new ByteMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(byte[] value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.ByteArrayMsg, new ByteArrayMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(Color value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.ColorMsg, new ColorMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(Color[] value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.ColorArrayMsg, new ColorArrayMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(Color32 value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.Color32Msg, new Color32Message(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(Color32[] value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.Color32ArrayMsg, new Color32ArrayMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(bool value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.BoolMsg, new BoolMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        public static void Send(bool[] value, string id = "", int qualityOfServiceChannel = 0, string address = "")
        {
            Send((short)NetworkMsg.BoolArrayMsg, new BoolArrayMessage(value, id, ipAddress), address, qualityOfServiceChannel);
        }

        //Loops:
        private void Update()
        {
            _server.Update();
        }

        //Event Handlers:
        private void HandleConnect(NetworkConnection connection)
        {
            _connections.Add(connection.connectionId, connection.address);
            if (OnPlayerConnected != null) OnPlayerConnected(connection.address);
        }

        private void HandleDisconnect(NetworkConnection connection)
        {
            //unity does not report the address of a disconnection this dictionary helps:
            string address = _connections[connection.connectionId];
            _connections.Remove(connection.connectionId);
            if (OnPlayerDisconnected != null) OnPlayerDisconnected(address);
        }

        private void HandleMatrix4x4(NetworkMessage message)
        {
            if (OnMatrix4x4 != null) OnMatrix4x4(message.ReadMessage<Matrix4x4Message>());
        }

        private void HandleMatrix4x4Array(NetworkMessage message)
        {
            if (OnMatrix4x4Array != null) OnMatrix4x4Array(message.ReadMessage<Matrix4x4ArrayMessage>());
        }

        private void HandleFloat(NetworkMessage message)
        {
            if (OnFloat != null) OnFloat(message.ReadMessage<FloatMessage>());
        }

        private void HandleFloatArray(NetworkMessage message)
        {
            if (OnFloatArray != null) OnFloatArray(message.ReadMessage<FloatArrayMessage>());
        }

        private void HandleInt(NetworkMessage message)
        {
            if (OnInt != null) OnInt(message.ReadMessage<IntMessage>());
        }

        private void HandleIntArray(NetworkMessage message)
        {
            if (OnIntArray != null) OnIntArray(message.ReadMessage<IntArrayMessage>());
        }

        private void HandleVector2(NetworkMessage message)
        {
            if (OnVector2 != null) OnVector2(message.ReadMessage<Vector2Message>());
        }

        private void HandleVector2Array(NetworkMessage message)
        {
            if (OnVector2Array != null) OnVector2Array(message.ReadMessage<Vector2ArrayMessage>());
        }

        private void HandleVector3(NetworkMessage message)
        {
            if (OnVector3 != null) OnVector3(message.ReadMessage<Vector3Message>());
        }

        private void HandleVector3Array(NetworkMessage message)
        {
            if (OnVector3Array != null) OnVector3Array(message.ReadMessage<Vector3ArrayMessage>());
        }

        private void HandleQuaternion(NetworkMessage message)
        {
            if (OnQuaternion != null) OnQuaternion(message.ReadMessage<QuaternionMessage>());
        }

        private void HandleQuaternionArray(NetworkMessage message)
        {
            if (OnQuaternionArray != null) OnQuaternionArray(message.ReadMessage<QuaternionArrayMessage>());
        }

        private void HandleVector4(NetworkMessage message)
        {
            if (OnVector4 != null) OnVector4(message.ReadMessage<Vector4Message>());
        }

        private void HandleVector4Array(NetworkMessage message)
        {
            if (OnVector4Array != null) OnVector4Array(message.ReadMessage<Vector4ArrayMessage>());
        }

        private void HandleRect(NetworkMessage message)
        {
            if (OnRect != null) OnRect(message.ReadMessage<RectMessage>());
        }

        private void HandleRectArray(NetworkMessage message)
        {
            if (OnRectArray != null) OnRectArray(message.ReadMessage<RectArrayMessage>());
        }

        private void HandleString(NetworkMessage message)
        {
            if (OnString != null) OnString(message.ReadMessage<StringMessage>());
        }

        private void HandleStringArray(NetworkMessage message)
        {
            if (OnStringArray != null) OnStringArray(message.ReadMessage<StringArrayMessage>());
        }

        private void HandleByte(NetworkMessage message)
        {
            if (OnByte != null) OnByte(message.ReadMessage<ByteMessage>());
        }

        private void HandleByteArray(NetworkMessage message)
        {
            if (OnByteArray != null) OnByteArray(message.ReadMessage<ByteArrayMessage>());
        }

        private void HandleColor(NetworkMessage message)
        {
            if (OnColor != null) OnColor(message.ReadMessage<ColorMessage>());
        }

        private void HandleColorArray(NetworkMessage message)
        {
            if (OnColorArray != null) OnColorArray(message.ReadMessage<ColorArrayMessage>());
        }

        private void HandleColor32(NetworkMessage message)
        {
            if (OnColor32 != null) OnColor32(message.ReadMessage<Color32Message>());
        }

        private void HandleColor32Array(NetworkMessage message)
        {
            if (OnColor32Array != null) OnColor32Array(message.ReadMessage<Color32ArrayMessage>());
        }

        private void HandleBool(NetworkMessage message)
        {
            if (OnBool != null) OnBool(message.ReadMessage<BoolMessage>());
        }

        private void HandleBoolArray(NetworkMessage message)
        {
            if (OnBoolArray != null) OnBoolArray(message.ReadMessage<BoolArrayMessage>());
        }
    }
}

#endif