using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;
using LANNetworking.Util;

namespace LANNetworking
{
	public class LANDiscovery : MonoBehaviour
	{
		public List<IPEndPoint> AvailableServers => Available.Select(a => a.Item2).ToList();

		private UdpClient Receiver;
		private readonly List<(float, IPEndPoint)> Available = new List<(float, IPEndPoint)>();
		[SerializeField, Range(1f, 50f), Tooltip("Factor which no broadcast is received from the server until the server is 'unavailabe'.")]
		private float BroadcastTolerace = 5f;

		private void Start()
		{
			Receiver = new UdpClient();
			Receiver.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			Receiver.Client.Bind(new IPEndPoint(IPAddress.Any, NetworkOptions.UdpPort));
		}

		private void Update()
		{
			Available.RemoveAll(t => t.Item1 + NetworkOptions.BroadcastInterval * BroadcastTolerace < Time.time);
			if (Receiver.Available > 0)
			{
				try
				{
					IPEndPoint ip = new IPEndPoint(IPAddress.Any, 0);
					byte[] payload = Receiver.Receive(ref ip);
					if (payload.Length == 8 && payload.ToLong() == NetworkOptions.GameId)
					{
						Available.RemoveAll(kvp => kvp.Item2.Equals(ip));
						Available.Add((Time.time, ip));
					}
				}
				catch (Exception e) {
					Debug.LogException(e);
				}
			}
		}
	}
}