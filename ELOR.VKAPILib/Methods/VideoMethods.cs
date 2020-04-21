using ELOR.VKAPILib.Attributes;
using ELOR.VKAPILib.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELOR.VKAPILib.Methods {
    [Section("video")]
    public class VideoMethods : MethodsSectionBase {
        internal VideoMethods(VKAPI api) : base(api) { }

        /// <summary>Returns a list of video albums owned by a user or community.</summary>
        /// <param name="ownerId">ID of the user or community that owns the video albums.</param>
        /// <param name="offset">Offset needed to return a specific subset of video albums.</param>
        /// <param name="count">Number of video albums to return.</param>
        /// <param name="extended">true — to return additional fields Count and Photo properties for each album.</param>
        /// <param name="needSystem">true — to return system albums.</param>
        [Method("getAlbums")]
        public async Task<VKList<VideoAlbum>> GetAlbumsAsync(int ownerId, int offset = 0, int count = 0, bool extended = false, bool needSystem = false) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("owner_id", ownerId.ToString());
            if (offset > 0) parameters.Add("offset", offset.ToString());
            if (count > 0) parameters.Add("count", count.ToString());
            if (extended) parameters.Add("extended", "1");
            if (needSystem) parameters.Add("need_system", "1");
            return await API.CallMethodAsync<VKList<VideoAlbum>>(this, parameters);
        }

        /// <summary>Returns a list of a user's or community's photos.</summary>
        /// <param name="ownerId">ID of the user or community that owns the videos.</param>
        /// <param name="albumId">ID of the album containing the videos.</param>
        /// <param name="offset">Offset needed to return a specific subset of videos.</param>
        /// <param name="count">Number of videos to return.</param>
        /// <param name="extended">true — to return an extended response with additional fields.</param>
        [Method("get")]
        public async Task<VKList<Video>> GetAsync(int ownerId, int albumId, int offset = 0, int count = 50, bool extended = false) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("owner_id", ownerId.ToString());
            parameters.Add("album_id", albumId.ToString());
            if(offset > 0) parameters.Add("offset", offset.ToString());
            if(count > 0) parameters.Add("count", count.ToString());
            if(extended) parameters.Add("extended", "1");
            return await API.CallMethodAsync<VKList<Video>>(this, parameters);
        }
    }
}
