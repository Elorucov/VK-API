using ELOR.VKAPILib.Attributes;
using ELOR.VKAPILib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ELOR.VKAPILib.Methods {
    public enum FriendsOrder { 
        [EnumMember(Value = "hints")]
        Hints, 
        
        [EnumMember(Value = "random")]
        Random, 

        [EnumMember(Value = "mobile")]
        Mobile, 

        [EnumMember(Value = "name")]
        Name,
    }

    [Section("friends")]
    public class FriendsMethods : MethodsSectionBase {
        internal FriendsMethods(VKAPI api) : base(api) { }

        [Method("get")]
        public async Task<VKList<User>> GetAsync(int userId = 0, FriendsOrder order = FriendsOrder.Hints, int listId = 0, int count = 5000, int offset = 0, List<string> fields = null, NameCase nameCase = NameCase.Nom) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (userId > 0) parameters.Add("user_id", userId.ToString());
            parameters.Add("order", order.ToEnumMemberAttribute());
            if (listId > 0) parameters.Add("list_id", listId.ToString());
            parameters.Add("count", count > 0 ? listId.ToString() : "5000");
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            parameters.Add("name_case", nameCase.ToEnumMemberAttribute());
            return await API.CallMethodAsync<VKList<User>>(this, parameters);
        }
    }
}
