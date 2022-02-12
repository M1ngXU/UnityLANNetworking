using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LANNetworking
{
	public class NetworkTransform : MonoBehaviour
	{
		[Range(1f, 20f)]
		public int BufferSize = 4;
		[Range(0, 100)]
		public int BufferUpdatesPerSecond = 20;
		public bool Show = false;

		private float BufferRefreshTime => 1f / BufferUpdatesPerSecond;
		/// <summary>
		/// List containing dt and the local position.
		/// </summary>
		private readonly List<(float, Vector3)> Positions = new List<(float, Vector3)>();
		private float LastUpdate = 0;

		// Update is called once per frame
		private void FixedUpdate()
		{
			float dt = Time.time - LastUpdate;
			if (dt < BufferRefreshTime) return;
			LastUpdate = Time.fixedTime;
			while (Positions.Count >= BufferSize) Positions.RemoveAt(0);
			Positions.Add((dt, transform.localPosition));
			if (Show)
			{
				Show = false;

				Debug.Log(string.Join("\n", GetData()));
			}
		}

		/// <summary>
		/// Returns the a List with { s(t), s'(t), s''(t), ... } with a length of <see cref="BufferSize"/>.
		/// </summary>
		public List<Vector3> GetData()
		{
			List<Vector3> data = new List<Vector3>();
			(float, Vector3)[] buffer = Positions.ToArray();
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