using LANNetworking.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

namespace LANNetworking
{
	public class NetworkTransformSyncServer : MonoBehaviour
	{
		public UdpClient Server;
		[SerializeField, Tooltip("Port for the server to send/receive broadcasts/network-transform-syncs.")]
		private int Port = 7878;

		private void Start() => Server = new UdpClient(Port);

		private void Update()
		{
			foreach (NetworkTransform nt in FindObjectsOfType<NetworkTransform>().Where(nt => nt.Data != null))
			{
				Debug.Log("update of " + nt.Id);
				Server.Send(nt.Id.GetBytes().Concat(nt.Data).ToArray(), 2 + nt.Data.Length, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8877));
				nt.Data = null;
			}
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(NetworkTransformSyncServer))]
	public class NetworkTransformSyncServerInspector : Editor
	{
		private SerializedProperty Port;

		private void OnEnable() => Port = serializedObject.FindProperty("Port");

		public override void OnInspectorGUI()
		{
			EditorGUI.BeginDisabledGroup(Application.isPlaying);
			EditorGUILayout.PropertyField(Port);
			EditorGUI.EndDisabledGroup();
			serializedObject.ApplyModifiedProperties();
		}
	}
#endif
}