using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

public class req_account
{
	public string sid;
}

public class ack_account
{
	public readonly string sid;
	public readonly long uid;
}

//abstract class AbstractPacketBase
//{
//}

public static class Protocols
{
	public abstract class ParamBase<TReq, TAck>// : AbstractPacketBase
	{
		public TReq reqData = (TReq)Activator.CreateInstance(typeof(TReq));
		public TAck ackData = default(TAck);	// Fill ackdata by Reflection.
		public abstract string protocolName { get; }
	}

	public class CheckAccount : ParamBase<req_account, ack_account>
	{
		public override string protocolName { get { return "account_r"; } }
	}
}

class Response<T>
{
	int ret = 0;
	T data = default(T);
}

public class TestObject : MonoBehaviour 
{
	public delegate void OnPostSuccess<T>(T ackData);

	// Use this for initialization
	void Start () 
	{
		//var packet = new Protocols.CheckAccount();
		//packet.reqData.sid = "afjeilaf";
		//Post(packet, OnSuccess);

		//var reqData = new req_account();
		//reqData.sid = "afeafads";

		//Post(reqData, OnSuccess);
	}

	void Post<TReq, TAck>(Protocols.ParamBase<TReq, TAck> packet, object onSuccess) //where T : AbstractPacketBase
	{
//		Debug.Log ("Packet Type :" + packet.GetType ().ToString () + " protocolName[" + packet.protocolName + "]");
//		var successFuncType = typeof(Action<>).MakeGenericType (new Type[] { typeof(TAck) });
//
//		var successFunc = Convert.ChangeType(onSuccess, successFuncType);
	}

	void Post<TReq, TAck>(TReq packet, Action<TAck> onSuccess)
	{
	}

//	void Post<T>(T packet, Action<T> onSuccess) where T : class
//	{
//		packet.reqData.sid = 0;
//	}

	void OnSuccess(ack_account packet)
	{

	}

	// Update is called once per frame
	void Update () {
	
	}
}
