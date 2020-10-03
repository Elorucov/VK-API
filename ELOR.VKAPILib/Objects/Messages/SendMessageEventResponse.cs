using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELOR.VKAPILib.Objects.Messages {
    public class SendMessageEventResponse {

        [JsonProperty("event_id")]
        public string EventId { get; internal set; }
    }
}
