using System;
using System.Linq;
using UnityEngine;

namespace LANNetworking.Util
{
	public static class BigEndianConverter
	{
		public static bool ToBoolean(this byte[] b, int i = 0) => BitConverter.ToBoolean(b.ReverseIfLittleEndian(), i);
		public static char ToChar(this byte[] b, int i = 0) => BitConverter.ToChar(b.ReverseIfLittleEndian(), i);
		public static double ToDouble(this byte[] b, int i = 0) => BitConverter.ToDouble(b.ReverseIfLittleEndian(), i);
		public static float ToFloat(this byte[] b, int i = 0) => BitConverter.ToSingle(b.ReverseIfLittleEndian(), i);
		public static short ToShort(this byte[] b, int i = 0) => BitConverter.ToInt16(b.ReverseIfLittleEndian(), i);
		public static int ToInt(this byte[] b, int i = 0) => BitConverter.ToInt32(b.ReverseIfLittleEndian(), i);
		public static long ToLong(this byte[] b, int i = 0) => BitConverter.ToInt64(b.ReverseIfLittleEndian(), i);
		public static ushort ToUShort(this byte[] b, int i = 0) => BitConverter.ToUInt16(b.ReverseIfLittleEndian(), i);
		public static uint ToUInt(this byte[] b, int i = 0) => BitConverter.ToUInt32(b.ReverseIfLittleEndian(), i);
		public static ulong ToULong(this byte[] b, int i = 0) => BitConverter.ToUInt64(b.ReverseIfLittleEndian(), i);
		public static Vector3 ToVector3(this byte[] b, int i = 0) => new Vector3(
			b.Skip(i).Take(4).ToArray().ToFloat(),
			b.Skip(i + 4).Take(4).ToArray().ToFloat(),
			b.Skip(i + 8).Take(4).ToArray().ToFloat()
		);

		public static byte[] GetBytes(this bool v) => BitConverter.GetBytes(v).ReverseIfLittleEndian();
		public static byte[] GetBytes(this char v) => BitConverter.GetBytes(v).ReverseIfLittleEndian();
		public static byte[] GetBytes(this double v) => BitConverter.GetBytes(v).ReverseIfLittleEndian();
		public static byte[] GetBytes(this short v) => BitConverter.GetBytes(v).ReverseIfLittleEndian();
		public static byte[] GetBytes(this int v) => BitConverter.GetBytes(v).ReverseIfLittleEndian();
		public static byte[] GetBytes(this long v) => BitConverter.GetBytes(v).ReverseIfLittleEndian();
		public static byte[] GetBytes(this float v) => BitConverter.GetBytes(v).ReverseIfLittleEndian();
		public static byte[] GetBytes(this ushort v) => BitConverter.GetBytes(v).ReverseIfLittleEndian();
		public static byte[] GetBytes(this ulong v) => BitConverter.GetBytes(v).ReverseIfLittleEndian();
		public static byte[] GetBytes(this Vector3 v) => v.x.GetBytes().Concat(v.y.GetBytes()).Concat(v.z.GetBytes()).ToArray();

		public static string ToByteString(this byte[] v, int i = 0, int l = int.MaxValue) => BitConverter.ToString(v.ReverseIfLittleEndian(), i, Math.Min(l, v.Length));

		private static byte[] ReverseIfLittleEndian(this byte[] b)
		{
			if (BitConverter.IsLittleEndian) Array.Reverse(b);
			return b;
		}
	}
}