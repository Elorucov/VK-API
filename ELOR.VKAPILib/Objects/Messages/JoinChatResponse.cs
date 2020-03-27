using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELOR.VKAPILib.Objects.Messages {
    public class JoinChatResponse {
        [JsonProperty("link")]
        public string Link { get; internal set; }
    }
}
