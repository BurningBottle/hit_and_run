using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PacketQueue
{
	struct PacketInfo
	{
		public int offset;
		public int size;
	}

	MemoryStream streamBuffer;
	List<PacketInfo> offsetList;
	int offset = 0;

	object lockObj = new object();

	public PacketQueue()
	{
		streamBuffer = new MemoryStream();
		offsetList = new List<PacketInfo>();
	}

	public int Enqueue(byte[] data, int size)
	{
		PacketInfo info = new PacketInfo();

		info.offset = offset;
		info.size = size;

		lock(lockObj)
		{
			offsetList.Add(info);
			streamBuffer.Position = offset;
			streamBuffer.Write(data, 0, size);
			streamBuffer.Flush();
			offset += size;
		}

		return size;
	}

	public int Dequeue(ref byte[] buffer, int size)
	{
		if (offsetList.Count <= 0)
			return -1;

		int recvSize = 0;
		lock(lockObj)
		{
			var info = offsetList[0];

			int dataSize = Mathf.Min(size, info.size);
			streamBuffer.Position = info.offset;
			recvSize = streamBuffer.Read(buffer, 0, dataSize);

			if (recvSize > 0)
				offsetList.RemoveAt(0);

			if(offsetList.Count == 0)
			{
				Clear();
				offset = 0;
			}
		}

		return recvSize;
	}

	public void Clear()
	{
		var buffer = streamBuffer.GetBuffer();
		System.Array.Clear(buffer, 0, buffer.Length);

		streamBuffer.Position = 0;
		streamBuffer.SetLength(0);
	}
}
