﻿using UnityEngine;
using System.Collections;

namespace UnitySocketIO.SocketIO {
	[System.Serializable]
	public class SocketIOSettings {

		public string url;
		public int port;

		public bool sslEnabled;

		public int reconnectTime;

		public int timeToDropAck;
		
		public int pingTimeout;
		public int pingInterval;
		public string username { get; set; }
		public string token { get; set; }

	}
}