using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELOR.VKAPILib.Objects.Common;

namespace ELOR.VKAPILib.Objects {
    public interface IPreview {
        Uri PreviewImageUri { get; }
        Size PreviewImageSize { get; }
    }
}
