
namespace SaxoServiceGroupsModels
{
    public struct SubscriptionModel
    {
        public Arguments Arguments { get; set; }
        public string ContextId { get; set; }
        public string ReferenceId {get;set;}
        public int RefreshRate { get; set; }
    }
    public struct Arguments
    {
        public string AssetType { get; set; }
        public int Uic { get; set; }

    }
    /// <summary>
	/// The WebSocketMessage is the data envelope returned by the streaming server. 
	/// The message contains meta data describing the message and where it comes from.
	/// Also included is the actual data, in the Payload property. 
	/// </summary>
	public struct WebSocketMessage
	{
		/// <summary>
		/// Message id that is unique within a streaming session.
		/// </summary>
		public long MessageId { get; set; }

		/// <summary>
		/// A unique subscription identifier. This is the reference id provided when the subscription was created.
		/// </summary>
		public string ReferenceId { get; set; }
		
		/// <summary>
		/// The message payload format. This is always Json, which has the value 0.
		/// </summary>
		public int PayloadFormat { get; set; }

		/// <summary>
		/// The payload is the actual data message. For Json this is a byte array representation of a UTF-8 encoded string.
		/// </summary>
		public byte[] Payload { get; set; }
	}
    public class HeartbeatControlMessage
	{
		public string ReferenceId { get; set; }
		public Heartbeat[] Heartbeats { get; set; }
	}

	public class Heartbeat
	{
		public string OriginatingReferenceId { get; set; }
		public string Reason { get; set; }
	}

	public class ResetSubscriptionsControlMessage
	{
		public string ReferenceId { get; set; }
		public string[] TargetReferenceIds { get; set; }
	}

	public class DisconnectControlMessage
	{
		public string ReferenceId { get; set; }
	}
}