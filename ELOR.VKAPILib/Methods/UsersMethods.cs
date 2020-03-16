using ELOR.VKAPILib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ELOR.VKAPILib.Methods {

    [DataContract]
    public enum NameCase {
        [EnumMember(Value = "nom")]
        Nom,

        [EnumMember(Value = "gen")]
        Gen,

        [EnumMember(Value = "dat")]
        Dat,

        [EnumMember(Value = "acc")]
        Acc,

        [EnumMember(Value = "ins")]
        Ins,

        [EnumMember(Value = "abl")]
        Abl
    }

    public class UsersMethods {
        internal VKAPI API;

        internal UsersMethods(VKAPI api) {
            API = api;
        }

        public async Task<List<User>> GetAsync(List<int> ids, List<string> fields = null, NameCase nameCase = NameCase.Nom) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (!ids.IsNullOrEmpty()) parameters.Add("user_ids", ids.Combine());
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            parameters.Add("name_case", nameCase.ToEnumMemberAttribute());
            return await API.CallMethodAsync<List<User>>("users.get", parameters);
        }

        public async Task<User> GetAsync(int id = 0, List<string> fields = null, NameCase nameCase = NameCase.Nom) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (id != 0) parameters.Add("user_ids", id.ToString());
            if (!fields.IsNullOrEmpty()) parameters.Add("fields", fields.Combine());
            parameters.Add("name_case", nameCase.ToEnumMemberAttribute());
            return (await API.CallMethodAsync<List<User>>("users.get", parameters)).First();
        }
    }
}
