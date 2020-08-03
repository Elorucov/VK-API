﻿using ELOR.VKAPILib.Attributes;
using ELOR.VKAPILib.Objects;
using ELOR.VKAPILib.Objects.Messages;
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

    [DataContract]
    public enum HistoryAttachmentMediaType {
        [EnumMember(Value = "photo")]
        Photo,

        [EnumMember(Value = "video")]
        Video,

        [EnumMember(Value = "audio")]
        Audio,

        [EnumMember(Value = "doc")]
        Doc,

        [EnumMember(Value = "link")]
        Link
    }

    [DataContract]
    public enum MessageIntent {
        None,

        [EnumMember(Value = "promo_newsletter")]
        PromoNewsletter,

        [EnumMember(Value = "bot_ad_invite")]
        BotAdInvite,

        [EnumMember(Value = "bot_ad_promo")]
        BotAdPromo
    }

    [DataContract]
    public enum ActivityType {
        [EnumMember(Value = "typing")]
        Typing,

        [EnumMember(Value = "audiomessage")]
        Audiomessage
    }

    [Section("messages")]
    public class MessagesMethods : MethodsSectionBase {
        internal MessagesMethods(VKAPI api) : base(api) { }

        /// <summary>Adds a new user to a chat.</summary>
        /// <param name="chatId">Chat ID.</param>
        /// <param name="userId">ID of the user to be added to the chat.</param>
        /// <param name="visibleMessagesCount">Visible messages count.</param>
        [Method("addChatUser")]
        public async Task<bool> AddChatUserAsync(int chatId, int userId, int visibleMessagesCount) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("chat_id", chatId.ToString());
            parameters.Add("user_id", userId.ToString());
            parameters.Add("visible_messages_count", visibleMessagesCount.ToString());
            return await API.CallMethodAsync<bool>(this, parameters);
        }

        /// <summary>Allows sending messages from community to the current user.</summary>
        /// <param name="groupId">Group ID.</param>
        /// <param name="key">Random string, can be used for the user identification. It returns with message_allow event in Callback API.</param>
        [Method("allowMessagesFromGroup")]
        public async Task<bool> AllowMessagesFromGroupAsync(int groupId, string key = "") {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("group_id", groupId.ToString());
            parameters.Add("key", key);
            return await API.CallMethodAsync<bool>(this, parameters);
        }

        /// <summary>Creates a chat with several participants.</summary>
        /// <param name="groupId">Group ID.</param>
        /// <param name="userIds">IDs of the users to be added to the chat.</param>
        /// <param name="title">Chat title.</param>
        [Method("createChat")]
        public async Task<int> CreateChatAsync(int groupId, List<int> userIds, string title = "") {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("user_ids", userIds.Combine());
            parameters.Add("title", title);
            return await API.CallMethodAsync<int>(this, parameters);
        }

        /// <summary>Deletes one or more messages.</summary>
        /// <param name="groupId">Group ID.</param>
        /// <param name="messageIds">Message IDs.</param>
        /// <param name="spam">true — to mark message as spam.</param>
        /// <param name="deleteForAll">true — to delete message for all (in 24 hours from the sending time).</param>
        [Method("delete")]
        public async Task<bool> DeleteAsync(int groupId, List<int> messageIds, bool spam, bool deleteForAll) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("message_ids", messageIds.Combine());
            if (spam) parameters.Add("spam", "1");
            if (deleteForAll) parameters.Add("delete_for_all", "1");
            return await API.CallMethodAsync<bool>(this, parameters);
        }

        /// <summary>Deletes a chat's cover picture.</summary>
        /// <param name="groupId">Group ID.</param>
        /// <param name="chatId">Chat ID.</param>
        [Method("deleteChatPhoto")]
        public async Task<SetChatPhotoResponse> DeleteChatPhotoAsync(int groupId, int chatId) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("chat_id", chatId.ToString());
            return await API.CallMethodAsync<SetChatPhotoResponse>(this, parameters);
        }

        /// <summary>Deletes private messages in a conversation.</summary>
        /// <param name="peerId">Destination ID.</param>
        [Method("deleteConversation")]
        public async Task<bool> DeleteConversationAsync(int peerId) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("peer_id", peerId.ToString());
            return await API.CallMethodAsync<bool>(this, parameters);
        }

        /// <summary>Denies sending message from community to the current user.</summary>
        /// <param name="groupId">Group ID.</param>
        [Method("denyMessagesFromGroup")]
        public async Task<bool> DenyMessagesFromGroupAsync(int groupId) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("group_id", groupId.ToString());
            return await API.CallMethodAsync<bool>(this, parameters);
        }

        /// <summary>Edits the message. You can edit sent message during 24 hours.</summary>
        /// <param name="groupId">Group ID.</param>
        /// <param name="peerId">Destination ID.</param>
        /// <param name="messageId">Message ID.</param>
        /// <param name="message">(Required if attachment parameter is null.) Text of the message.</param>
        /// <param name="latitude">Geographical latitude of a check-in, in degrees (from -90 to 90).</param>
        /// <param name="longitude">Geographical longitude of a check-in, in degrees (from -180 to 180).</param>
        /// <param name="attachment">(Required if message parameter is null.) List of objects attached to the message.</param>
        /// <param name="keepForwardedMessages">true — to keep forwarded, messages.</param>
        /// <param name="keepSnippets">true — to keep attached snippets.</param>
        /// <param name="dontParseLinks">Don't parse links in message.</param>
        [Method("edit")]
        public async Task<bool> EditAsync(int groupId, int peerId, int messageId, string message, double latitude, double longitude, List<string> attachment, bool keepForwardedMessages, bool keepSnippets, bool dontParseLinks) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("peer_id", peerId.ToString());
            parameters.Add("message_id", messageId.ToString());
            if (!String.IsNullOrEmpty(message)) parameters.Add("message", message);
            if (latitude > 0) parameters.Add("lat", latitude.ToString());
            if (longitude > 0) parameters.Add("long", longitude.ToString());
            if (!attachment.IsNullOrEmpty()) parameters.Add("attachment", attachment.Combine());
            if (keepForwardedMessages) parameters.Add("keep_forward_messages", "1");
            if (keepSnippets) parameters.Add("keep_snippets", "1");
            if (dontParseLinks) parameters.Add("dont_parse_links", "1");
            return await API.CallMethodAsync<bool>(this, parameters);
        }

        /// <summary>Edits the title of a chat.</summary>
        /// <param name="chatId">Chat ID.</param>
        /// <param name="title">New title of the chat.</param>
        [Method("editChat")]
        public async Task<bool> EditChatAsync(int chatId, string title) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("chat_id", chatId.ToString());
            parameters.Add("title", title);
            return await API.CallMethodAsync<bool>(this, parameters);
        }

        /// <summary>Returns messages by their ids as part of a conversation.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="conversationMessageIds">Conversation message IDs.</param>
        /// <param name="extended">true – return additional information about users and communities in users and communities fields.</param>
        /// <param name="fields">List of additional fields for users and communities.</param>
        [Method("getByConversationMessageId")]
        public async Task<VKList<Message>> GetByConversationMessageIdAsync(int groupId, List<int> conversationMessageIds, bool extended = false, List<string> fields = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("conversation_message_ids", conversationMessageIds.Combine());
            if (extended) parameters.Add("extended", "1");
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return await API.CallMethodAsync<VKList<Message>>(this, parameters);
        }

        /// <summary>Returns messages by their IDs.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="messageIds">Message IDs.</param>
        /// <param name="extended">true – return additional information about users and communities in users and communities fields.</param>
        /// <param name="fields">List of additional fields for users and communities.</param>
        [Method("getById")]
        public async Task<VKList<Message>> GetByIdAsync(int groupId, List<int> messageIds, int previewLength, bool extended = false, List<string> fields = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("message_ids", messageIds.Combine());
            if (previewLength > 0) parameters.Add("preview_length", previewLength.ToString());
            if (extended) parameters.Add("extended", "1");
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return await API.CallMethodAsync<VKList<Message>>(this, parameters);
        }

        /// <summary>Returns information about a chat.</summary>
        /// <param name="chatId">Chat ID.</param>
        /// <param name="fields">List of additional fields for users and communities.</param>
        /// <param name="nameCase">Case for declension of user name and surname.</param>
        [Method("getChat")]
        public async Task<VKList<Chat>> GetChatAsync(int chatId, List<string> fields = null, NameCase nameCase = NameCase.Nom) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("chat_id", chatId.ToString());
            if(!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            parameters.Add("name_case", nameCase.ToEnumMemberAttribute());
            return await API.CallMethodAsync<VKList<Chat>>(this, parameters);
        }

        /// <summary>Returns information about a chat.</summary>
        /// <param name="chatIds">Chat IDs.</param>
        /// <param name="fields">List of additional fields for users and communities.</param>
        /// <param name="nameCase">Case for declension of user name and surname.</param>
        [Method("getChat")]
        public async Task<VKList<Chat>> GetChatAsync(List<int> chatIds, List<string> fields = null, NameCase nameCase = NameCase.Nom) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("chat_ids", chatIds.Combine());
            if(!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            parameters.Add("name_case", nameCase.ToEnumMemberAttribute());
            return await API.CallMethodAsync<VKList<Chat>>(this, parameters);
        }

        /// <summary>Allows to receive chat preview by the invitation link.</summary>
        /// <param name="link">Invitation link.</param>
        /// <param name="fields">List of additional fields for users and communities.</param>
        [Method("getChatPreview")]
        public async Task<ChatPreview> GetChatPreviewAsync(string link, List<string> fields = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("link", link);
            if(!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return await API.CallMethodAsync<ChatPreview>(this, parameters);
        }

        /// <summary>Returns a list of IDs of users participating in a conversation.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="peerId">Destination ID.</param>
        /// <param name="fields">List of additional fields for users and communities.</param>
        [Method("getConversationMembers")]
        public async Task<VKList<ChatMember>> GetConversationMembersAsync(int groupId, int peerId, List<string> fields) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if(groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("peer_id", peerId.ToString());
            if(!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return await API.CallMethodAsync<VKList<ChatMember>>(this, parameters);
        }

        /// <summary>Returns a list of conversations.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="fields">List of additional fields for users and communities.</param>
        /// <param name="filter">Types of conversations to return.</param>
        /// <param name="count">Number of conversations to return.</param>
        /// <param name="offset">Offset needed to return a specific subset of conversations.</param>
        /// <param name="extended">true – return additional information about users and communities in users and communities fields.</param>
        [Method("getConversations")]
        public async Task<ConversationsResponse> GetConversationsAsync(int groupId, List<string> fields, ConversationsFilter filter, bool extended = false, int count = 60, int offset = 0) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("filter", filter.ToEnumMemberAttribute());
            parameters.Add("count", count.ToString());
            parameters.Add("offset", offset.ToString());
            if (extended) parameters.Add("extended", "1");
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return await API.CallMethodAsync<ConversationsResponse>(this, parameters);
        }

        /// <summary>Returns conversations by their IDs.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="peerIds">list of destination IDs.</param>
        /// <param name="extended">true – return additional information about users and communities in users and communities fields.</param>
        /// <param name="fields">List of additional fields for users and communities.</param>
        [Method("getConversationsById")]
        public async Task<VKList<Conversation>> GetConversationsByIdAsync(int groupId, List<int> peerIds, bool extended = false, List<string> fields = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("peer_ids", peerIds.Combine());
            if (extended) parameters.Add("extended", "1");
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return await API.CallMethodAsync<VKList<Conversation>>(this, parameters);
        }

        /// <summary>Returns message history for the specified user or group chat.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="peerId">Peer ID.</param>
        /// <param name="offset">Offset needed to return a specific subset of messages.</param>
        /// <param name="count">Number of messages to return.</param>
        /// <param name="startMessageId">Starting message ID from which to return history.</param>
        /// <param name="extended">true – return additional information about users and communities in users and communities fields.</param>
        /// <param name="fields">List of additional fields for users and communities.</param>
        /// <param name="rev">Sort order.</param>
        [Method("getHistory")]
        public async Task<MessagesHistoryResponse> GetHistoryAsync(int groupId, int peerId, int offset, int count, int startMessageId, bool extended = false, List<string> fields = null, bool rev = false) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("peer_id", peerId.ToString());
            parameters.Add("offset", offset.ToString());
            parameters.Add("count", count.ToString());
            parameters.Add("start_message_id", startMessageId.ToString());
            if (rev) parameters.Add("rev", "1");
            if (extended) parameters.Add("extended", "1");
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return await API.CallMethodAsync<MessagesHistoryResponse>(this, parameters);
        }

        /// <summary>Returns media files from the dialog or group chat.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="peerId">Peer ID.</param>
        /// <param name="mediaType">Type of media files to return.</param>
        /// <param name="startFrom">Message ID to start return results from.</param>
        /// <param name="count">Number of messages to return.</param>
        /// <param name="photoSizes">true — to return photo sizes.</param>
        /// <param name="fields">List of additional fields for users and communities.</param>
        /// <param name="preserveOrder">Preserve order.</param>
        /// <param name="maxForwardsLevel">Preserve order.</param>
        [Method("getHistoryAttachments")]
        public async Task<ConversationAttachmentsResponse> GetHistoryAttachmentsAsync(int groupId, int peerId, HistoryAttachmentMediaType mediaType, int startFrom, int count, bool photoSizes, bool preserveOrder = false, int maxForwardsLevel = 45, List<string> fields = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("peer_id", peerId.ToString());
            parameters.Add("media_type", mediaType.ToEnumMemberAttribute());
            parameters.Add("start_from", startFrom.ToString());
            parameters.Add("count", count.ToString());
            if (photoSizes) parameters.Add("photo_sizes", "1");
            if (preserveOrder) parameters.Add("preserve_order", "1");
            parameters.Add("max_forwards_level", maxForwardsLevel.ToString());
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return await API.CallMethodAsync<ConversationAttachmentsResponse>(this, parameters);
        }

        /// <summary>Returns a list of user's important messages.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="offset">Offset needed to return a specific subset of messages.</param>
        /// <param name="count">Number of messages to return.</param>
        /// <param name="startMessageId">Starting message ID from which to return list.</param>
        /// <param name="previewLength">Preview length.</param>
        /// <param name="extended">true – return additional information about users and communities in users and communities fields.</param>
        /// <param name="fields">List of additional fields for users and communities.</param>
        [Method("getImportantMessages")]
        public async Task<VKList<Message>> GetImportantMessagesAsync(int groupId, int offset, int count, int startMessageId, int previewLength = 0, bool extended = false, List<string> fields = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("offset", offset.ToString());
            parameters.Add("count", count.ToString());
            parameters.Add("start_message_id", startMessageId.ToString());
            if (previewLength > 0) parameters.Add("preview_length", previewLength.ToString());
            if (extended) parameters.Add("extended", "1");
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return await API.CallMethodAsync<VKList<Message>>(this, parameters);
        }

        /// <summary>Receives a link to invite a user to the chat.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="peerId">Peer ID.</param>
        /// <param name="reset">true — to generate new link (revoke previous).</param>
        [Method("getInviteLink")]
        public async Task<ChatLink> GetInviteLinkAsync(int groupId, int peerId, bool reset = false) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("peer_id", peerId.ToString());
            if (reset) parameters.Add("reset", "1");
            return await API.CallMethodAsync<ChatLink>(this, parameters);
        }

        /// <summary>Returns a user's current status and date of last activity.</summary>
        /// <param name="userId">User ID.</param>
        [Method("getLastActivity")]
        public async Task<LastActivity> GetLastActivityAsync(int userId) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("user_id", userId.ToString());
            return await API.CallMethodAsync<LastActivity>(this, parameters);
        }

        /// <summary>Returns updates in user's private messages.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="ts">Last value of the ts parameter returned from the Long Poll server.</param>
        /// <param name="pts">Last value of pts parameter returned from the Long Poll server.</param>
        /// <param name="previewLength">Number of characters after which to truncate a previewed message. To preview the full message, specify 0.</param>
        /// <param name="onlines">true — to return history with online users only.</param>
        /// <param name="fields">Additional profile fileds to return.</param>
        /// <param name="eventsLimit">Maximum number of events to return. (minimum 1000)</param>
        /// <param name="msgsLimit">Maximum number of messages to return. (minimum 200)</param>
        /// <param name="maxMsgId">Maximum ID of the message among existing ones in the local copy.</param>
        [Method("getLongPollHistory")]
        public async Task<LongPollHistoryResponse> GetLongPollHistoryAsync(int groupId, int ts, int pts, int previewLength, bool onlines, int eventsLimit = 1000, int msgsLimit = 200, int maxMsgId = 0, List<string> fields = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if(groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("lp_version", API.LongPollVersion.ToString());
            parameters.Add("ts", ts.ToString());
            parameters.Add("pts", pts.ToString());
            if (previewLength > 0) parameters.Add("preview_length", previewLength.ToString());
            if (onlines) parameters.Add("onlines", "1");
            parameters.Add("events_limit", eventsLimit.ToString());
            parameters.Add("msgs_limit", msgsLimit.ToString());
            if (maxMsgId > 0) parameters.Add("mmax_msg_id", maxMsgId.ToString());
            parameters.Add("credentials", "1");
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return await API.CallMethodAsync<LongPollHistoryResponse>(this, parameters);
        }

        /// <summary>Returns data required for connection to a Long Poll server.</summary>
        /// <param name="needPts">true — to return the pts field, needed for the GetLongPollHistoryAsync method.</param>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        [Method("getLongPollServer")]
        public async Task<LongPollServerInfo> GetLongPollServerAsync(bool needPts, int groupId) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            if (needPts) parameters.Add("need_pts", "1");
            parameters.Add("lp_version", API.LongPollVersion.ToString());
            return await API.CallMethodAsync<LongPollServerInfo>(this, parameters);
        }

        /// <summary>Returns a list of recently used graffities.</summary>
        /// <param name="limit">Group ID.</param>
        [Method("getRecentGraffities")]
        public async Task<List<Document>> GetRecentGraffitiesAsync(int limit = 20) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("limit", limit.ToString());
            return await API.CallMethodAsync<List<Document>>(this, parameters);
        }

        /// <summary>Returns a list of recently used stickers.</summary>
        [Method("getRecentStickers")]
        public async Task<VKList<Sticker>> GetRecentStickersAsync() {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            return await API.CallMethodAsync<VKList<Sticker>>(this, parameters);
        }

        /// <summary>Returns information whether sending messages from the community to current user is allowed.</summary>
        /// <param name="groupId">Group ID.</param>
        /// <param name="userId">User ID.</param>
        [Method("isMessagesFromGroupAllowed")]
        public async Task<IsAllowedResponse> IsMessagesFromGroupAllowedAsync(int groupId, int userId) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("group_id", groupId.ToString());
            parameters.Add("user_id", userId.ToString());
            return await API.CallMethodAsync<IsAllowedResponse>(this, parameters);
        }

        /// <summary>Allows to enter the chat by the invitation link.</summary>
        /// <param name="link">Invitation link.</param>
        [Method("joinChatByInviteLink")]
        public async Task<JoinChatResponse> JoinChatByInviteLinkAsync(string link) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("link", link);
            return await API.CallMethodAsync<JoinChatResponse>(this, parameters);
        }

        /// <summary>Marks/unmarks the conversation as answered.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="peerId">Peer ID.</param>
        /// <param name="answered">true — to mark conversation as answered, false — unmark.</param>
        [Method("markAsAnsweredConversation")]
        public async Task<bool> MarkAsAnsweredConversationAsync(int groupId, int peerId, bool answered) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("peer_id", peerId.ToString());
            parameters.Add("answered", answered ? "1" : "0");
            return await API.CallMethodAsync<bool>(this, parameters);
        }

        /// <summary>Marks and unmarks messages as important (starred).</summary>
        /// <param name="messageIds">IDs of messages to mark as important.</param>
        /// <param name="important">true — to add a star (mark as important), false — to remove the star.</param>
        [Method("markAsImportant")]
        public async Task<List<int>> MarkAsImportantAsync(List<int> messageIds, bool important) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("message_ids", messageIds.Combine());
            parameters.Add("important", important ? "1" : "0");
            return await API.CallMethodAsync<List<int>>(this, parameters);
        }

        /// <summary>Marks/unmarks the conversation as important.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="peerId">Peer ID.</param>
        /// <param name="important">true — to mark conversation as important, false — unmark.</param>
        [Method("markAsImportantConversation")]
        public async Task<bool> MarkAsImportantConversationAsync(int groupId, int peerId, bool important) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("peer_id", peerId.ToString());
            parameters.Add("important", important ? "1" : "0");
            return await API.CallMethodAsync<bool>(this, parameters);
        }

        /// <summary>Marks messages as read.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="messageIds">IDs of messages to mark as read.</param>
        /// <param name="peerId">Destination ID.</param>
        [Method("markAsRead")]
        public async Task<bool> MarkAsReadAsync(int groupId, int peerId, int startMessageId) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("peer_id", peerId.ToString());
            parameters.Add("start_message_id", startMessageId.ToString());
            return await API.CallMethodAsync<bool>(this, parameters);
        }

        /// <summary>Pins a message.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="peerId">Destination ID.</param>
        /// <param name="messageId">ID of message to pin.</param>
        [Method("pin")]
        public async Task<Message> PinAsync(int groupId, int peerId, int messageId) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("peer_id", peerId.ToString());
            parameters.Add("message_id", messageId.ToString());
            return await API.CallMethodAsync<Message>(this, parameters);
        }

        /// <summary>Allows the current user to leave a chat or, if the current user started the chat, allows the user to remove another user from the chat.</summary>
        /// <param name="chatId">Chat ID.</param>
        /// <param name="memberId">ID of the member to be removed.</param>
        [Method("removeChatUser")]
        public async Task<bool> RemoveChatUserAsync(int chatId, int memberId) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("chat_id", chatId.ToString());
            parameters.Add("member_id", memberId.ToString());
            return await API.CallMethodAsync<bool>(this, parameters);
        }

        /// <summary>Restores a deleted message.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="messageId">ID of a previously-deleted message to restore.</param>
        [Method("restore")]
        public async Task<bool> RestoreAsync(int groupId, int messageId) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("message_id", messageId.ToString());
            return await API.CallMethodAsync<bool>(this, parameters);
        }

        /// <summary>Returns a list of the current user's private messages that match search criteria.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="query">Search query.</param>
        /// <param name="peerId">Destination ID.</param>
        /// <param name="date">Date to search messages have been sent before it.</param>
        /// <param name="previewLength">Number of characters after which to truncate a previewed message. To preview the full message, specify 0.</param>
        /// <param name="offset">Offset needed to return a specific subset of messages.</param>
        /// <param name="count">Number of messages to return.</param>
        /// <param name="extended">true — to return additional profiles and groups array with users and communities objects.</param>
        /// <param name="fields">List of additional fields for profiles and communities to be returned.</param>
        [Method("search")]
        public async Task<VKList<Message>> SearchAsync(int groupId, string query, int peerId, DateTime? date = null, int previewLength = 0, int offset = 0, int count = 20, bool extended = false, List<string> fields = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("q", query);
            if (peerId > 0) parameters.Add("peer_id", peerId.ToString());
            if (date != null) parameters.Add("date", date.Value.ToVKFormat());
            if (previewLength > 0) parameters.Add("preview_length", previewLength.ToString());
            parameters.Add("offset", offset.ToString());
            parameters.Add("count", count.ToString());
            if (extended) parameters.Add("extended", "1");
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return await API.CallMethodAsync<VKList<Message>>(this, parameters);
        }

        /// <summary>Returns a list of conversations that match search criteria.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="query">Search query.</param>
        /// <param name="count">Maximum number of results.</param>
        /// <param name="extended">true — return additional fields.</param>
        /// <param name="fields">List of additional fields for profiles and communities to be returned.</param>
        [Method("searchConversations")]
        public async Task<VKList<Conversation>> SearchConversationsAsync(int groupId, string query, int count = 20, bool extended = false, List<string> fields = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("q", query);
            parameters.Add("count", count.ToString());
            if(extended) parameters.Add("extended", "1");
            if(!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return await API.CallMethodAsync<VKList<Conversation>>(this, parameters);
        }

        /// <summary>Sends a message.</summary>
        /// <param name="groupId">Group ID.</param>
        /// <param name="peerId">Destination ID.</param>
        /// <param name="randomId">Unique identifier to avoid resending the message.</param>
        /// <param name="message">(Required if attachment parameter is null.) Text of the message.</param>
        /// <param name="latitude">Geographical latitude of a check-in, in degrees (from -90 to 90).</param>
        /// <param name="longitude">Geographical longitude of a check-in, in degrees (from -180 to 180).</param>
        /// <param name="attachment">(Required if message parameter is null.) List of objects attached to the message.</param>
        /// <param name="replyTo">Id of replied message.</param>
        /// <param name="forwardMessages">ID of forwarded messages. Listed messages of the sender will be shown in the message body at the recipient's.</param>
        /// <param name="stickerId">Sticker id.</param>
        /// <param name="keyboard">Keyboard (for bots).</param>
        /// <param name="payload">Payload of message.</param>
        /// <param name="dontParseLinks">true — links will not attach snippet.</param>
        /// <param name="disableMentions">true — mention of user will not generate notification for him.</param>
        [Method("send")]
        public async Task<int> SendAsync(int groupId, int peerId, int randomId, string message, double latitude, double longitude, List<string> attachment, int replyTo, List<int> forwardMessages, int stickerId, string keyboard = null, string payload = null, bool dontParseLinks = false, bool disableMentions = false, MessageIntent intent = MessageIntent.None) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("peer_id", peerId.ToString());
            parameters.Add("random_id", randomId.ToString());
            if (!String.IsNullOrEmpty(message)) parameters.Add("message", message);
            if (latitude > 0) parameters.Add("lat", latitude.ToString());
            if (longitude > 0) parameters.Add("long", longitude.ToString());
            if (!attachment.IsNullOrEmpty()) parameters.Add("attachment", attachment.Combine());
            if (replyTo > 0) parameters.Add("reply_to", replyTo.ToString());
            if (!forwardMessages.IsNullOrEmpty()) parameters.Add("forward_messages", forwardMessages.Combine());
            if (stickerId > 0) parameters.Add("sticker_id", stickerId.ToString());
            if (!String.IsNullOrEmpty(keyboard)) parameters.Add("keyboard", keyboard); // TODO for bots: Parse keyboard object instead of string
            if (!String.IsNullOrEmpty(payload)) parameters.Add("payload", payload);
            if (dontParseLinks) parameters.Add("dont_parse_links", "1");
            if (disableMentions) parameters.Add("disable_mentions", "1");
            if (intent != MessageIntent.None) parameters.Add("intent", intent.ToEnumMemberAttribute()); // Untested new bots feature
            return await API.CallMethodAsync<int>(this, parameters);
        }

        /// <summary>Changes the status of a user as typing in a conversation.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="peerId">Destination ID.</param>
        /// <param name="type">Activity type (Typing — user has started to type, Audiomessage — user has started to record audiomessage).</param>
        [Method("setActivity")]
        public async Task<bool> SetActivityAsync(int groupId, int peerId, ActivityType type) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("peer_id", peerId.ToString());
            parameters.Add("type", type.ToEnumMemberAttribute());
            return await API.CallMethodAsync<bool>(this, parameters);
        }

        /// <summary>Sets a previously-uploaded picture as the cover picture of a chat.</summary>
        /// <param name="file">Upload URL from the response field returned by the Photos.GetChatUploadServerAsync method upon successfully uploading an image.</param>
        [Method("setChatPhoto")]
        public async Task<SetChatPhotoResponse> SetChatPhotoAsync(string file) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("file", file);
            return await API.CallMethodAsync<SetChatPhotoResponse>(this, parameters);
        }

        /// <summary>Unpins a message.</summary>
        /// <param name="groupId">Group ID (for community messages with a user access token).</param>
        /// <param name="peerId">Destination ID.</param>
        [Method("unpin")]
        public async Task<Message> UnpinAsync(int groupId, int peerId) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if(groupId > 0) parameters.Add("group_id", groupId.ToString());
            parameters.Add("peer_id", peerId.ToString());
            return await API.CallMethodAsync<Message>(this, parameters);
        }
    }
}