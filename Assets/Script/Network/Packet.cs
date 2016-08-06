using UnityEngine;
using System.Collections;

public class HeaderSerializer : Serializer
{
	public bool Serialize(PacketHeader data)
	{
		// 기존 데이터를 클리어합니다.
		Clear();

		// 각 요소를 사례로 시리얼라이즈합니다.
		bool ret = true;
		ret &= Serialize((int)data.packetId);

		if (ret == false)
		{
			return false;
		}

		return true;
	}


	public bool Deserialize(byte[] data, ref PacketHeader serialized)
	{
		// 디시리얼라이즈할 데이터를 설정합니다.
		bool ret = SetDeserializedData(data);
		if (ret == false)
		{
			return false;
		}

		// 데이터의 요소별로 디시리얼라이즈합니다.
		int packetId = 0;
		ret &= Deserialize(ref packetId);
		serialized.packetId = (PacketId)packetId;

		return ret;
	}
}

public enum PacketId
{
	LoadingComplete,
	GameStart,
}

public struct PacketHeader
{
	public PacketId packetId;
}

public interface IPacket<T>
{
	// 
	PacketId GetPacketId();

	//	
	T GetPacket();

	//
	byte[] GetData();
}

public struct LoadingCompleteData
{
	public int playerId;
}

public class LoadingCompletePacket : IPacket<LoadingCompleteData>
{
	class ItemSerializer : Serializer
	{
		//
		public bool Serialize(LoadingCompleteData packet)
		{
			bool ret = true;
			ret &= Serialize(packet.playerId);

			return ret;
		}

		//
		public bool Deserialize(ref LoadingCompleteData element)
		{
			if (GetDataSize() == 0)
			{
				// 데이터가 설정되어 있지 않습니다.
				return false;
			}

			bool ret = true;
			ret &= Deserialize(ref element.playerId);

			return ret;
		}
	}

	// 패킷 데이터의 실체.
	LoadingCompleteData m_packet;
	
	// 패킷 데이터를 시리얼라이즈 하는 생성자.
	public LoadingCompletePacket(LoadingCompleteData data)
	{
		m_packet = data;
	}

	// 바이너리 데이터를 패킷 데이터로 디시리얼라이즈 하는 생성자. 
	public LoadingCompletePacket(byte[] data)
	{
		ItemSerializer serializer = new ItemSerializer();

		serializer.SetDeserializedData(data);
		serializer.Deserialize(ref m_packet);
	}

	public PacketId GetPacketId()
	{
		return PacketId.LoadingComplete;
	}

	// 게임에서 사용할 패킷 데이터를 획득.
	public LoadingCompleteData GetPacket()
	{
		return m_packet;
	}

	// 송신용 byte[]형 데이터를 획득.
	public byte[] GetData()
	{
		ItemSerializer serializer = new ItemSerializer();
		serializer.Serialize(m_packet);

		return serializer.GetSerializedData();
	}
}

public struct GameStartData
{
	public int randomSeed;
}

public class GameStartPacket : IPacket<GameStartData>
{
	class ItemSerializer : Serializer
	{
		//
		public bool Serialize(GameStartData packet)
		{
			bool ret = true;
			ret &= Serialize(packet.randomSeed);

			return ret;
		}

		//
		public bool Deserialize(ref GameStartData element)
		{
			if (GetDataSize() == 0)
			{
				// 데이터가 설정되어 있지 않습니다.
				return false;
			}

			bool ret = true;
			ret &= Deserialize(ref element.randomSeed);

			return ret;
		}
	}

	// 패킷 데이터의 실체.
	GameStartData m_packet;

	// 패킷 데이터를 시리얼라이즈 하는 생성자.
	public GameStartPacket(GameStartData data)
	{
		m_packet = data;
	}

	// 바이너리 데이터를 패킷 데이터로 디시리얼라이즈 하는 생성자. 
	public GameStartPacket(byte[] data)
	{
		ItemSerializer serializer = new ItemSerializer();

		serializer.SetDeserializedData(data);
		serializer.Deserialize(ref m_packet);
	}

	public PacketId GetPacketId()
	{
		return PacketId.GameStart;
	}

	// 게임에서 사용할 패킷 데이터를 획득.
	public GameStartData GetPacket()
	{
		return m_packet;
	}

	// 송신용 byte[]형 데이터를 획득.
	public byte[] GetData()
	{
		ItemSerializer serializer = new ItemSerializer();
		serializer.Serialize(m_packet);

		return serializer.GetSerializedData();
	}
}

