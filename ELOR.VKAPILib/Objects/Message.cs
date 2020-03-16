using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ELOR.VKAPILib.Objects {
    public class MessagesHistoryResponse {
        [JsonProperty("items")]
        public List<Message> Items { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("profiles")]
        public List<User> Profiles { get; set; }

        [JsonProperty("groups")]
        public List<Group> Groups { get; set; }

        [JsonProperty("conversations")]
        public List<Conversation> Conversations { get; set; }
    }

    //

    public class GeoCoordinates {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }

    public class GeoPlace {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("created")]
        public int Created { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }
    }

    public class Geo {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public GeoCoordinates Coordinates { get; set; }

        [JsonProperty("place")]
        public GeoPlace Place { get; set; }
    }

    //

    [DataContract]
    public enum ActionType {
        [EnumMember(Value = "chat_photo_update")]
        ChatPhotoUpdate,

        [EnumMember(Value = "chat_photo_remove")]
        ChatPhotoRemove,

        [EnumMember(Value = "chat_create")]
        ChatCreate,

        [EnumMember(Value = "chat_title_update")]
        ChatTitleUpdate,

        [EnumMember(Value = "chat_invite_user")]
        ChatInviteUser,

        [EnumMember(Value = "chat_kick_user")]
        ChatKickUser,

        [EnumMember(Value = "chat_pin_message")]
        ChatPinMessage,

        [EnumMember(Value = "chat_unpin_message")]
        ChatUnpinMessage,

        [EnumMember(Value = "chat_invite_user_by_link")]
        ChatInviteUserByLink,
    }

    public class Action {
        [JsonProperty("type")]
        public ActionType Type { get; set; }

        [JsonProperty("member_id")]
        public int MemberId { get; set; }

        [JsonIgnore]
        public int FromId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Message {
        [JsonIgnore]
        public DateTime DateTime { get { return DateTimeOffset.FromUnixTimeSeconds(DateUnix).DateTime.ToLocalTime(); } }

        [JsonIgnore]
        public DateTime UpdateTime { get { return DateTimeOffset.FromUnixTimeSeconds(UpdateTimeUnix).DateTime.ToLocalTime(); } }

        //

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("conversation_message_id")]
        public int ConversationMessageId { get; set; }

        [JsonProperty("date")]
        public long DateUnix { get; set; }

        [JsonProperty("update_time")]
        public long UpdateTimeUnix { get; set; }

        [JsonProperty("peer_id")]
        public int PeerId { get; set; }

        [JsonProperty("from_id")]
        public int FromId { get; set; }

        [JsonProperty("admin_author_id")]
        public int AdminAuthorId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("random_id")]
        public int RandomId { get; set; }

        [JsonProperty("attachments")]
        public List<Attachment> Attachments { get; set; }

        [JsonProperty("important")]
        public bool Important { get; set; }

        [JsonProperty("geo")]
        public Geo Geo { get; set; }

        [JsonProperty("payload")]
        public string PayLoad { get; set; }

        [JsonProperty("keyboard")]
        public BotKeyboard Keyboard { get; set; }

        [JsonProperty("fwd_messages")]
        public List<Message> ForwardedMessages { get; set; }

        [JsonProperty("reply_message")]
        public Message ReplyMessage { get; set; }

        [JsonProperty("action")]
        public Action Action { get; set; }

        [JsonProperty("template")]
        public BotTemplate Template { get; set; }
    }
}
