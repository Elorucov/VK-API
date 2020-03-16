using ELOR.VKAPILib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ELOR.VKAPILib.Methods {

    [DataContract]
    public enum ConversationsFilter {
        [EnumMember(Value = "all")]
        All,

        [EnumMember(Value = "unread")]
        Unread,

        [EnumMember(Value = "important")]
        Important,

        [EnumMember(Value = "unanswered")]
        Unanswered,

        [EnumMember(Value = "message_request")]
        MessageRequest,

        [EnumMember(Value = "business_notify")]
        BusinessNotify
    }

    public class MessagesMethods : MethodsSectionBase {
        internal MessagesMethods(VKAPI api, string section) : base(api, section) { }

        /// <summary>Returns a list of conversations.</summary>
        /// <param name="chatId">Chat ID.</param>
        /// <param name="userId">ID of the user to be added to the chat.</param>
        /// <param name="visibleMessagesCount">Visible messages count.</param>
        public async Task<bool> AddChatUserAsync(int chatId, int userId, int visibleMessagesCount) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("chat_id", chatId.ToString());
            parameters.Add("user_id", userId.ToString());
            parameters.Add("visible_messages_count", visibleMessagesCount.ToString());
            return await API.CallMethodAsync<bool>($"{Section}.addChatUser", parameters);
        }

        /// <summary>Allows sending messages from community to the current user.</summary>
        /// <param name="groupId">Group ID.</param>
        /// <param name="key">Random string, can be used for the user identification. It returns with message_allow event in Callback API.</param>
        public async Task<bool> AllowMessagesFromGroupAsync(int groupId, string key = "") {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("group_id", groupId.ToString());
            parameters.Add("key", key);
            return await API.CallMethodAsync<bool>($"{Section}.allowMessagesFromGroup", parameters);
        }

        /// <summary>Creates a chat with several participants.</summary>
        /// <param name="groupId">Group ID.</param>
        /// <param name="userIds">IDs of the users to be added to the chat.</param>
        /// <param name="title">Chat title.</param>
        public async Task<int> CreateChatAsync(int groupId, List<int> userIds, string title = "") {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("group_id", groupId.ToString());
            parameters.Add("user_ids", userIds.Combine());
            parameters.Add("title", title);
            return await API.CallMethodAsync<int>($"{Section}.createChat", parameters);
        }

        /// <summary>Deletes one or more messages.</summary>
        /// <param name="groupId">Group ID.</param>
        /// <param name="messageIds">Message IDs.</param>
        /// <param name="spam">true — to mark message as spam.</param>
        /// <param name="deleteForAll">true — to delete message for all (in 24 hours from the sending time).</param>
        public async Task<bool> DeleteAsync(int groupId, List<int> messageIds, bool spam, bool deleteForAll) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("group_id", groupId.ToString());
            parameters.Add("message_ids", messageIds.Combine());
            if (spam) parameters.Add("spam", "1");
            if (deleteForAll) parameters.Add("delete_for_all", "1");
            return await API.CallMethodAsync<bool>($"{Section}.delete", parameters);
        }

        /// <summary>Returns a list of conversations.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="fields">List of additional fields for users and communities.</param>
        /// <param name="filter">Types of conversations to return.</param>
        /// <param name="count">Number of conversations to return.</param>
        /// <param name="offset">Offset needed to return a specific subset of conversations.</param>
        public async Task<ConversationsResponse> GetConversationsAsync(int groupId, List<string> fields, ConversationsFilter filter, int count = 60, int offset = 0) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("filter", filter.ToEnumMemberAttribute());
            parameters.Add("count", count.ToString());
            parameters.Add("offset", offset.ToString());
            parameters.Add("extended", "1");
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return await API.CallMethodAsync<ConversationsResponse>($"{Section}.getConversations", parameters);
        }

        /// <summary>Returns message history for the specified user or group chat.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="peerId">Peer ID.</param>
        /// <param name="offset">Offset needed to return a specific subset of messages.</param>
        /// <param name="count">Number of messages to return.</param>
        /// <param name="startMessageId">Starting message ID from which to return history.</param>
        /// <param name="fields">List of additional fields for users and communities.</param>
        /// <param name="rev">Sort order.</param>
        public async Task<MessagesHistoryResponse> GetHistoryAsync(int groupId, int peerId, int offset, int count, int startMessageId, List<string> fields, bool rev) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("peer_id", peerId.ToString());
            parameters.Add("offset", offset.ToString());
            parameters.Add("count", count.ToString());
            parameters.Add("start_message_id", startMessageId.ToString());
            if (rev) parameters.Add("rev", "1");
            parameters.Add("extended", "1");
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return await API.CallMethodAsync<MessagesHistoryResponse>($"{Section}.getHistory", parameters);
        }

        /// <summary>Returns data required for connection to a Long Poll server.</summary>
        /// <param name="needPts">true — to return the pts field, needed for the messages.getLongPollHistory method.</param>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="LPVersion">Long Poll version. Actual version is 10.</param>
        public async Task<LongPollServerInfo> GetLongPollServerAsync(bool needPts, int groupId, int LPVersion = 10) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            if (needPts) parameters.Add("need_pts", "1");
            parameters.Add("lp_version", LPVersion.ToString());
            return await API.CallMethodAsync<LongPollServerInfo>($"{Section}.getLongPollServer", parameters);
        }
    }
}
