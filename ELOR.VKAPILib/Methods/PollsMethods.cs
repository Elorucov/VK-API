using ELOR.VKAPILib.Attributes;
using ELOR.VKAPILib.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELOR.VKAPILib.Methods {
    [Section("polls")]
    public class PollsMethods : MethodsSectionBase {
        internal PollsMethods(VKAPI api) : base(api) { }

        /// <summary>Return default backgrounds for polls.</summary>
        [Method("getBackgrounds")]
        public async Task<List<PollBackground>> GetBackgroundsAsync() {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            return await API.CallMethodAsync<List<PollBackground>>(this, parameters);
        }
    }
}
