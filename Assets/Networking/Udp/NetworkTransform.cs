using LANNetworking.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LANNetworking
{
	[ExecuteAlways]
	public class NetworkTransform : MonoBehaviour
	{
		[SerializeField, Range(1f, 20f)]
		private int BufferSize = 4;
		[SerializeField, Range(0, 100)]
		private int BufferUpdatesPerSecond = 20;
		[Tooltip("Non-zero network id.")]
		public ushort Id;
		
		public byte[] Data;

		private float BufferRefreshTime => 1f / BufferUpdatesPerSecond;
		/// <summary>
		/// List containing dt and the local position.
		/// </summary>
		private List<Vector3> Positions = new List<Vector3>();
		private readonly List<float> Times = new List<float>();
		private float LastUpdate = 0;
		private byte[] LastData;
		private bool IsServer;

		private void Start()
		{
			if (Id == 0) Id = (ushort)Random.Range(ushort.MinValue, ushort.MaxValue);
			IsServer = FindObjectOfType<NetworkTransformSyncServer>() != null;
		}

		// Update is called once per frame
		private void FixedUpdate()
		{
			if (!Application.isPlaying) return;
			if (IsServer)
			{
				float dt = Time.time - LastUpdate;
				if (dt < BufferRefreshTime) return;
				LastUpdate = Time.fixedTime;
				while (Positions.Count >= BufferSize) Positions.RemoveAt(0);
				while (Times.Count >= BufferSize) Times.RemoveAt(0);
				Positions.Add(transform.localPosition);
				Times.Add(dt);
				byte[] CurrentData = GetData().SelectMany(v => v.GetBytes()).ToArray();

				if (LastData == null || !Enumerable.SequenceEqual(CurrentData, LastData)) Data = CurrentData;
				LastData = CurrentData;
			} else
			{
				if (Data != null)
				{
					Positions.Clear();
					int offset = 0;
					while (offset < Data.Length)
					{
						Positions.Add(Data.Skip(offset).Take(4 * 3).ToArray().ToVector3());
						offset += 4 * 3;
					}
					Data = null;
				}
				for (int i = Positions.Count - 2; i >= 0; i--)
				{
					Positions[i] += Positions[i + 1] * Time.deltaTime;
				}
				if (Positions.Count > 0) transform.position = Positions[0];
			}
		}

		/// <summary>
		/// Returns the a List with { s(t), s'(t), s''(t), ... } with a length of <see cref="BufferSize"/>.
		/// </summary>
		private List<Vector3> GetData()
		{
			List<Vector3> data = new List<Vector3>();
			(float, Vector3)[] buffer = Times.Zip(Positions, (p, t) => (p, t)).ToArray();
			data.Add(buffer[0].Item2);
			while (buffer.Length > 1)
			{
				(float, Vector3)[] new_buffer = buffer.Skip(1).Select((v, i) => (v.Item1, (v.Item2 - buffer[i].Item2) / v.Item1)).ToArray();
				data.Add(new_buffer.Last().Item2);
				buffer = new_buffer;
			}
			return data;
		}
	}
}