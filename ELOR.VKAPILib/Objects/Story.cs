using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ELOR.VKAPILib.Objects {
    [DataContract]
    public enum StoryType {
        [EnumMember(Value = "photo")]
        Photo,

        [EnumMember(Value = "video")]
        Video
    }

    public class StoryLink {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonIgnore]
        public Uri Uri { get { return new Uri(Url); } }
    }

    public class Story : AttachmentBase {
        [JsonIgnore]
        public new string ObjectType { get { return "story"; } }

        [JsonProperty("can_see")]
        public int CanSee { get; set; }

        [JsonProperty("can_share")]
        public int CanShare { get; set; }

        [JsonProperty("is_resricted")]
        public bool IsRestricted { get; set; }

        [JsonProperty("is_expired")]
        public bool IsExpired { get; set; }

        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("views")]
        public int Views { get; set; }

        [JsonProperty("seen")]
        public int Seen { get; set; }

        [JsonProperty("link")]
        public StoryLink Link { get; set; }

        [JsonProperty("type")]
        public StoryType Type { get; set; }

        [JsonProperty("photo")]
        public Photo Photo { get; set; }

        [JsonProperty("video")]
        public Video Video { get; set; }
    }
}
