using LANNetworking.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace LANNetworking
{
	public class NetworkTransformSyncClient : MonoBehaviour
	{
		private UdpClient Receiver;

		void Start() => Receiver = new UdpClient(8877);

		private void Update()
		{
			if (Receiver.Available > 0)
			{
				IPEndPoint a = new IPEndPoint(0, 0);
				byte[] content = Receiver.Receive(ref a);

				ushort id = content.Take(2).ToArray().ToUShort();
				FindObjectsOfType<NetworkTransform>().Where(nt => nt.Id == id).First().Data = content.Skip(2).ToArray();
			}
		}
	}
}