using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELOR.VKAPILib.Objects {
    public class PhotoAlbum : Album {
        
        [JsonProperty("size")]
        public int Size { get; set; }
    }
}
