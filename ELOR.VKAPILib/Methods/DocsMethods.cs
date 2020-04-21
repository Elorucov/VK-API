using ELOR.VKAPILib.Attributes;
using ELOR.VKAPILib.Objects;
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
    }
}
