using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELOR.VKAPILib.Objects
{
    public class Call
    {
        [JsonProperty("initiator_id")]
        public int InitiatorId { get; set; }

        [JsonProperty("receiver_id")]
        public int ReceiverId { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }
}
