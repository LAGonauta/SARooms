/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
///
/// Example and utility for how to handle available servers.
/// 
/// NOTE: Communication does not support Unity's new networking system introduced in Unity 2019 - a new version will be created soon.
///
/// </summary>

#if !UNITY_2019

using System.Collections.Generic;
using UnityEngine;

namespace Pixelplacement
{
    public class ClientConnector : MonoBehaviour
    {
        //Private Classes:
        private class AvailableServer
        {
            //Public Variables:
            public string ip;
            public int port;
            public string deviceID;

            //constructors:
            public AvailableServer(string ip, int port, string deviceID)
            {
                this.ip = ip;
                this.port = port;
                this.deviceID = deviceID;
            }
        }

        //Private Variables:
        [SerializeField] private bool _connectToFirstAvailable = true;
        [SerializeField] private string _requiredDeviceId = "";
        private List<AvailableServer> _availableServers = new List<AvailableServer>();
        private bool _cleanUp;
        private float _lastCleanUpTime;
        private float _cleanUpTimeout = 4;
        private float _cleanUpTimeoutBackup = .5f;

        //Init:
        private void Awake()
        {
            Client.OnConnected += HandleConnected;
            Client.OnDisconnected += HandleDisconnected;
            Client.OnServerAvailable += HandleServerAvailable;
            HandleDisconnected();
        }

        //Private Methods:
        private void CleanUp()
        {
            //if no server entires have come in for a while clean up the server list:
            if (Time.realtimeSinceStartup - _lastCleanUpTime > _cleanUpTimeout + _cleanUpTimeoutBackup)
            {
                _lastCleanUpTime = Time.realtimeSinceStartup;
                _availableServers.Clear();
            }
            else
            {
                //or flag a regualr clean up pass:
                _cleanUp = true;
            }
        }

        //Event Handlers:
        private void HandleConnected()
        {
            CancelInvoke("CleanUp");
        }

        private void HandleDisconnected()
        {
            InvokeRepeating("CleanUp", 0, _cleanUpTimeout);
        }

        private void HandleServerAvailable(ServerAvailableMessage message)
        {
            if (!string.IsNullOrEmpty(_requiredDeviceId))
            {
                if (message.deviceId != _requiredDeviceId) return;
            }

            //auto connect:
            if (_connectToFirstAvailable)
            {
                Client.Connect(message.fromIp, message.port);
            }

            //clear out servers list to ensure we don't have any dead entries:
            if (_cleanUp)
            {
                _cleanUp = false;
                _lastCleanUpTime = Time.realtimeSinceStartup;
                _availableServers.Clear();
            }

            //only add if new:
            foreach (var item in _availableServers)
            {
                if (item.ip == message.fromIp) return;
            }

            //add this new server:
            _availableServers.Add(new AvailableServer(message.fromIp, message.port, message.deviceId));
        }

        //GUI:
        private void OnGUI()
        {
            GUI.matrix = Matrix4x4.Scale(Vector3.one * 5);

            //we don't need a connection menu if we are connected:
            if (Client.Connected) return;

            //feedback and buttons:
            if (_availableServers.Count == 0)
            {
                GUILayout.Label("Waiting for servers...");
            }
            else
            {
                //don't show a menu if they want to just connect to the first available:
                if (_connectToFirstAvailable) return;

                //informative header:
                GUILayout.Label("Select a server:");

                //show connection buttons for each server:
                foreach (var item in _availableServers)
                {
                    if (GUILayout.Button(item.deviceID, GUILayout.ExpandWidth(false))) Client.Connect(item.ip, item.port);
                }
            }
        }
    }
}

#endif