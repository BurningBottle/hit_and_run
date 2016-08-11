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
	RunStart,
	KeyInput,
	Hit,
	ShotMissile,
	GameOver,
	RestartGame,
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

public struct RunStartData
{
	public bool isStart;
	public Vector3 position;
}

public class RunStartPacket : IPacket<RunStartData>
{
	class ItemSerializer : Serializer
	{
		//
		public bool Serialize(RunStartData packet)
		{
			bool ret = true;
			ret &= Serialize(packet.isStart);
			ret &= Serialize(packet.position.x);
			ret &= Serialize(packet.position.y);
			ret &= Serialize(packet.position.z);

			return ret;
		}

		//
		public bool Deserialize(ref RunStartData element)
		{
			if (GetDataSize() == 0)
			{
				// 데이터가 설정되어 있지 않습니다.
				return false;
			}

			bool ret = true;
			ret &= Deserialize(ref element.isStart);

			element.position = new Vector3();

			ret &= Deserialize(ref element.position.x);
			ret &= Deserialize(ref element.position.y);
			ret &= Deserialize(ref element.position.z);

			return ret;
		}
	}

	// 패킷 데이터의 실체.
	RunStartData m_packet;

	// 패킷 데이터를 시리얼라이즈 하는 생성자.
	public RunStartPacket(RunStartData data)
	{
		m_packet = data;
	}

	// 바이너리 데이터를 패킷 데이터로 디시리얼라이즈 하는 생성자. 
	public RunStartPacket(byte[] data)
	{
		ItemSerializer serializer = new ItemSerializer();

		serializer.SetDeserializedData(data);
		serializer.Deserialize(ref m_packet);
	}

	public PacketId GetPacketId()
	{
		return PacketId.RunStart;
	}

	// 게임에서 사용할 패킷 데이터를 획득.
	public RunStartData GetPacket()
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

public struct KeyInputData
{
	public Vector2 keyNormal;
}

public class KeyInputPacket : IPacket<KeyInputData>
{
	class ItemSerializer : Serializer
	{
		//
		public bool Serialize(KeyInputData packet)
		{
			bool ret = true;
			ret &= Serialize(packet.keyNormal.x);
			ret &= Serialize(packet.keyNormal.y);

			return ret;
		}

		//
		public bool Deserialize(ref KeyInputData element)
		{
			if (GetDataSize() == 0)
			{
				// 데이터가 설정되어 있지 않습니다.
				return false;
			}

			bool ret = true;
			element.keyNormal = new Vector2();

			ret &= Deserialize(ref element.keyNormal.x);
			ret &= Deserialize(ref element.keyNormal.y);

			return ret;
		}
	}

	// 패킷 데이터의 실체.
	KeyInputData m_packet;

	// 패킷 데이터를 시리얼라이즈 하는 생성자.
	public KeyInputPacket(KeyInputData data)
	{
		m_packet = data;
	}

	// 바이너리 데이터를 패킷 데이터로 디시리얼라이즈 하는 생성자. 
	public KeyInputPacket(byte[] data)
	{
		ItemSerializer serializer = new ItemSerializer();

		serializer.SetDeserializedData(data);
		serializer.Deserialize(ref m_packet);
	}

	public PacketId GetPacketId()
	{
		return PacketId.KeyInput;
	}

	// 게임에서 사용할 패킷 데이터를 획득.
	public KeyInputData GetPacket()
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

public struct HitData
{
	public Vector3 position;
}

public class HitPacket : IPacket<HitData>
{
	class ItemSerializer : Serializer
	{
		//
		public bool Serialize(HitData packet)
		{
			bool ret = true;
			ret &= Serialize(packet.position.x);
			ret &= Serialize(packet.position.y);
			ret &= Serialize(packet.position.z);

			return ret;
		}

		//
		public bool Deserialize(ref HitData element)
		{
			if (GetDataSize() == 0)
			{
				// 데이터가 설정되어 있지 않습니다.
				return false;
			}

			bool ret = true;
			element.position = new Vector3();

			ret &= Deserialize(ref element.position.x);
			ret &= Deserialize(ref element.position.y);
			ret &= Deserialize(ref element.position.z);

			return ret;
		}
	}

	// 패킷 데이터의 실체.
	HitData m_packet;

	// 패킷 데이터를 시리얼라이즈 하는 생성자.
	public HitPacket(HitData data)
	{
		m_packet = data;
	}

	// 바이너리 데이터를 패킷 데이터로 디시리얼라이즈 하는 생성자. 
	public HitPacket(byte[] data)
	{
		ItemSerializer serializer = new ItemSerializer();

		serializer.SetDeserializedData(data);
		serializer.Deserialize(ref m_packet);
	}

	public PacketId GetPacketId()
	{
		return PacketId.Hit;
	}

	// 게임에서 사용할 패킷 데이터를 획득.
	public HitData GetPacket()
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

public struct ShotMissileData
{
	public Vector3 from;
	public Vector3 dest;
}

public class ShotMissilePacket : IPacket<ShotMissileData>
{
	class ItemSerializer : Serializer
	{
		//
		public bool Serialize(ShotMissileData packet)
		{
			bool ret = true;
			ret &= Serialize(packet.from.x);
			ret &= Serialize(packet.from.y);
			ret &= Serialize(packet.from.z);
			ret &= Serialize(packet.dest.x);
			ret &= Serialize(packet.dest.y);
			ret &= Serialize(packet.dest.z);

			return ret;
		}

		//
		public bool Deserialize(ref ShotMissileData element)
		{
			if (GetDataSize() == 0)
			{
				// 데이터가 설정되어 있지 않습니다.
				return false;
			}

			bool ret = true;
			element.from = new Vector3();

			ret &= Deserialize(ref element.from.x);
			ret &= Deserialize(ref element.from.y);
			ret &= Deserialize(ref element.from.z);

			element.dest = new Vector3();

			ret &= Deserialize(ref element.dest.x);
			ret &= Deserialize(ref element.dest.y);
			ret &= Deserialize(ref element.dest.z);

			return ret;
		}
	}

	// 패킷 데이터의 실체.
	ShotMissileData m_packet;

	// 패킷 데이터를 시리얼라이즈 하는 생성자.
	public ShotMissilePacket(ShotMissileData data)
	{
		m_packet = data;
	}

	// 바이너리 데이터를 패킷 데이터로 디시리얼라이즈 하는 생성자. 
	public ShotMissilePacket(byte[] data)
	{
		ItemSerializer serializer = new ItemSerializer();

		serializer.SetDeserializedData(data);
		serializer.Deserialize(ref m_packet);
	}

	public PacketId GetPacketId()
	{
		return PacketId.ShotMissile;
	}

	// 게임에서 사용할 패킷 데이터를 획득.
	public ShotMissileData GetPacket()
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

public struct GameOverData
{
	public bool myPlayerDead;
	public Vector3 position;
}

public class GameOverPacket : IPacket<GameOverData>
{
	class ItemSerializer : Serializer
	{
		//
		public bool Serialize(GameOverData packet)
		{
			bool ret = true;
			ret &= Serialize(packet.myPlayerDead);
			ret &= Serialize(packet.position.x);
			ret &= Serialize(packet.position.y);
			ret &= Serialize(packet.position.z);

			return ret;
		}

		//
		public bool Deserialize(ref GameOverData element)
		{
			if (GetDataSize() == 0)
			{
				// 데이터가 설정되어 있지 않습니다.
				return false;
			}

			bool ret = true;
			ret &= Deserialize(ref element.myPlayerDead);

			element.position = new Vector3();

			ret &= Deserialize(ref element.position.x);
			ret &= Deserialize(ref element.position.y);
			ret &= Deserialize(ref element.position.z);

			return ret;
		}
	}

	// 패킷 데이터의 실체.
	GameOverData m_packet;

	// 패킷 데이터를 시리얼라이즈 하는 생성자.
	public GameOverPacket(GameOverData data)
	{
		m_packet = data;
	}

	// 바이너리 데이터를 패킷 데이터로 디시리얼라이즈 하는 생성자. 
	public GameOverPacket(byte[] data)
	{
		ItemSerializer serializer = new ItemSerializer();

		serializer.SetDeserializedData(data);
		serializer.Deserialize(ref m_packet);
	}

	public PacketId GetPacketId()
	{
		return PacketId.GameOver;
	}

	// 게임에서 사용할 패킷 데이터를 획득.
	public GameOverData GetPacket()
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

public struct RestartGameData
{
	public int playerId;
}

public class RestartGamePacket : IPacket<RestartGameData>
{
	class ItemSerializer : Serializer
	{
		//
		public bool Serialize(RestartGameData packet)
		{
			bool ret = true;
			ret &= Serialize(packet.playerId);

			return ret;
		}

		//
		public bool Deserialize(ref RestartGameData element)
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
	RestartGameData m_packet;

	// 패킷 데이터를 시리얼라이즈 하는 생성자.
	public RestartGamePacket(RestartGameData data)
	{
		m_packet = data;
	}

	// 바이너리 데이터를 패킷 데이터로 디시리얼라이즈 하는 생성자. 
	public RestartGamePacket(byte[] data)
	{
		ItemSerializer serializer = new ItemSerializer();

		serializer.SetDeserializedData(data);
		serializer.Deserialize(ref m_packet);
	}

	public PacketId GetPacketId()
	{
		return PacketId.RestartGame;
	}

	// 게임에서 사용할 패킷 데이터를 획득.
	public RestartGameData GetPacket()
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