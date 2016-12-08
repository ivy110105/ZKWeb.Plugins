﻿#if !NETCORE
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pingpp.Models {
	public class ChargeList : Pingpp {
		[JsonProperty("object")]
		public string Object { get; set; }
		[JsonProperty("url")]
		public string Url { get; set; }
		[JsonProperty("has_more")]
		public bool HasMore { get; set; }
		[JsonProperty("data")]
		public IEnumerable<Charge> Data { get; set; }
	}
}
#endif
