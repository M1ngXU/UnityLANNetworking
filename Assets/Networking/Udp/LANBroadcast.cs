using LANNetworking.Util;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace LANNetworking
{
	[RequireComponent(typeof(NetworkTransformSyncServer))]
	public class LANBroadcast : MonoBehaviour
	{
		private readonly float BroadcastInterval = 200;
		private float LastBroadcast = 0;
		private IPEndPoint Target;
		private NetworkTransformSyncServer NetworkTransformSyncServer;

		private void Start() {
			NetworkTransformSyncServer = GetComponent<NetworkTransformSyncServer>();
			Target = new IPEndPoint(IPAddress.Broadcast, NetworkOptions.UdpPort);
		}

		private void Update()
		{
			if (LastBroadcast + BroadcastInterval < Time.time) return;
			LastBroadcast = Time.time;
			NetworkTransformSyncServer.Server.EnableBroadcast = true;
			NetworkTransformSyncServer.Server.Send(NetworkOptions.GameId.GetBytes(), 8, Target);
		}
	}
}