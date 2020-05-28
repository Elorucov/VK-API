﻿using ELOR.VKAPILib.Attributes;
using ELOR.VKAPILib.Objects;
using ELOR.VKAPILib.Objects.Upload;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELOR.VKAPILib.Methods {
    [Section("docs")]
    public class DocsMethods : MethodsSectionBase {
        internal DocsMethods(VKAPI api) : base(api) { }

        /// <summary>Returns detailed information about user or community documents.</summary>
        /// <param name="ownerId">ID of the user or community that owns the documents.</param>
        /// <param name="type">Document type. See possible values at vk.com/dev/docs.get</param>
        /// <param name="offset">Offset needed to return a specific subset of photos.</param>
        /// <param name="count">Number of photos to return. </param>
        /// <param name="returnTags">true — to return tags.</param>
        [Method("get")]
        public async Task<VKList<Document>> GetAsync(int ownerId, int type = 0, int offset = 0, int count = 50, bool returnTags = false) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("owner_id", ownerId.ToString());
            parameters.Add("type", type.ToString());
            if (offset > 0) parameters.Add("offset", offset.ToString());
            if (count > 0) parameters.Add("count", count.ToString());
            if (returnTags) parameters.Add("return_tags", "1");
            return await API.CallMethodAsync<VKList<Document>>(this, parameters);
        }

        /// <summary>Returns the server address for document upload.</summary>
        /// <param name="peerId">Destination ID.</param>
        [Method("getMessagesUploadServer")]
        public async Task<VkUploadServer> GetMessagesUploadServerAsync(int groupId, int peerId, bool isAudioMessage = false) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (groupId > 0) parameters.Add("group_id", groupId.ToString());
            if (peerId > 0) parameters.Add("peer_id", peerId.ToString());
            if (isAudioMessage) parameters.Add("type", "audio_message");
            return await API.CallMethodAsync<VkUploadServer>(this, parameters);
        }

        /// <summary>Saves a document after uploading it to a server.</summary>
        [Method("save")]
        public async Task<DocumentSaveResult> SaveAsync(string file, string title = null, string tags = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("file", file);
            if (!String.IsNullOrEmpty(title)) parameters.Add("title", title);
            if (!String.IsNullOrEmpty(tags)) parameters.Add("tags", tags);
            return await API.CallMethodAsync<DocumentSaveResult>(this, parameters);
        }
    }
}
