using ELOR.VKAPILib.Attributes;
using ELOR.VKAPILib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELOR.VKAPILib.Methods {
    [Section("groups")]
    public class GroupsMethods : MethodsSectionBase {
        internal GroupsMethods(VKAPI api) : base(api) { }

        /// <summary>Returns a list of the communities to which a user belongs.</summary>
        /// <param name="userId">User ID.</param>
        /// <param name="extended">true — to return complete information about a user's communities.</param>
        /// <param name="fields">Group fields to return.</param>
        /// <param name="filter">Types of communities to return.</param>
        /// <param name="offset">Offset needed to return a specific subset of communities.</param>
        /// <param name="count">Number of communities to return.</param>
        [Method("get")]
        public async Task<VKList<Group>> GetAsync(int userId, bool extended = false, List<string> fields = null, List<string> filter = null, int offset = 0, int count = 1000) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("user_id", userId.ToString());
            if (extended) parameters.Add("extended", "1");
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            if (!filter.IsNullOrEmpty()) parameters.Add("filter", filter.Combine());
            parameters.Add("offset", offset.ToString());
            parameters.Add("count", count.ToString());
            return await API.CallMethodAsync<VKList<Group>>(this, parameters);
        }

        /// <summary>Returns information about communities by their IDs.</summary>
        /// <param name="groupIds">Group IDs.</param>
        /// <param name="fields">Group fields to return.</param>
        [Method("getById")]
        public async Task<List<Group>> GetByIdAsync(List<int> groupIds, List<string> fields = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("group_ids", groupIds.Combine());
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return await API.CallMethodAsync<List<Group>>(this, parameters);
        }

        /// <summary>Returns information about community by ID.</summary>
        /// <param name="groupId">Group ID.</param>
        /// <param name="fields">Group fields to return.</param>
        [Method("getById")]
        public async Task<Group> GetByIdAsync(int groupId, List<string> fields = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("group_id", groupId.ToString());
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return (await API.CallMethodAsync<List<Group>>(this, parameters)).First();
        }
    }
}
