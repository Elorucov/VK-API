using System;
using System.Collections.Generic;
using System.Text;

namespace ELOR.VKAPILib.Objects.Common {
    public class Size {
        public double Width { get; private set; }
        public double Height { get; private set; }

        internal Size(double width, double height) {
            Width = width;
            Height = height;
        }
    }
}
