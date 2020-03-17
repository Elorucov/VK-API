using ELOR.VKAPILib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELOR.VKAPILib.Methods {
    public class GroupsMethods : MethodsSectionBase {
        internal GroupsMethods(VKAPI api, string section) : base(api, section) { }

        public async Task<VKList<Group>> GetAsync(int userId, List<string> fields = null, List<string> filter = null, int count = 0, int offset = 0) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("user_id", userId.ToString());
            parameters.Add("extended", "1");
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            if (!filter.IsNullOrEmpty()) parameters.Add("filter", filter.Combine());
            parameters.Add("offset", offset.ToString());
            parameters.Add("count", count.ToString());
            return await API.CallMethodAsync<VKList<Group>>($"{Section}.get", parameters);
        }

        public async Task<List<Group>> GetByIdAsync(List<int> groupIds, List<string> fields = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("group_ids", groupIds.Combine());
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return await API.CallMethodAsync<List<Group>>($"{Section}.getById", parameters);
        }

        public async Task<Group> GetByIdAsync(int groupId, List<string> fields = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("group_id", groupId.ToString());
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            return (await API.CallMethodAsync<List<Group>>($"{Section}.getById", parameters)).First();
        }
    }
}
