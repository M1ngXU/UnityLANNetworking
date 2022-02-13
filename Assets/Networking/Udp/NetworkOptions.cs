using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LANNetworking
{
	[System.Serializable]
	public static class NetworkOptions
	{
		public static int UdpPort = 12345;
		public static int TcpPort => UdpPort - 1;
		public static float BroadcastInterval = 1f;
		public static long GameId = 11;
	}

#if UNITY_EDITOR
	public class NetworkOptionWindow : EditorWindow, ISerializationCallbackReceiver
	{
		#region Serialize Network Options
		[SerializeField] private int _UdpPort;
		[SerializeField] private float _BroadcastInterval;
		[SerializeField] private long _GameId;

		public void OnAfterDeserialize()
		{
			NetworkOptions.UdpPort = _UdpPort;
			NetworkOptions.BroadcastInterval = _BroadcastInterval;
			NetworkOptions.GameId = _GameId;
		}

		public void OnBeforeSerialize()
		{
			_UdpPort = NetworkOptions.UdpPort;
			_BroadcastInterval = NetworkOptions.BroadcastInterval;
			_GameId = NetworkOptions.GameId;
		}
		#endregion

		[MenuItem("Window/Networking Options")]
		public static void ShowWindow() => GetWindow<NetworkOptionWindow>("Networking options");

		private void OnGUI()
		{
			EditorGUI.BeginDisabledGroup(Application.isPlaying);

			NetworkOptions.UdpPort = EditorGUILayout.IntField(
				new GUIContent("Broadcast port", "Port where broadcasts/Network-Transform-Syncs are sent/received. A Game with another Game ID is ignored."),
				NetworkOptions.UdpPort
			);
			NetworkOptions.BroadcastInterval = EditorGUILayout.Slider(
				new GUIContent("Broadcast Interval", "At what interval (in seconds) should the server broadcast? (Maximum once per `Update`)\nIf the client didn't receive a broadcast from the server for that amount of time multiplied by the client's `BroadcastTolerance`, the server will be considered as `unavailable`."),
				NetworkOptions.BroadcastInterval, 0f, 10f
			);
			EditorGUILayout.Space();
			NetworkOptions.GameId = EditorGUILayout.LongField(
				new GUIContent("Game Id", "The id of the Game version, each Game version should have its own game id."),
				NetworkOptions.GameId
			);

			EditorGUI.EndDisabledGroup();
		}
	}
#endif
}