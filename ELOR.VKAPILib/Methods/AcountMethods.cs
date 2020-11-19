using ELOR.VKAPILib.Attributes;
using ELOR.VKAPILib.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELOR.VKAPILib.Methods {

    [Section("account")]
    public class AccountMethods : MethodsSectionBase {
        internal AccountMethods(VKAPI api) : base(api) { }

        [Method("getPrivacySettings")]
        public async Task<PrivacyResponse> GetPrivacySettingsAsync() {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            return await API.CallMethodAsync<PrivacyResponse>(this, parameters);
        }
    }
}
