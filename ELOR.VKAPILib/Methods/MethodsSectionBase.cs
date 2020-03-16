using System;
using System.Collections.Generic;
using System.Text;

namespace ELOR.VKAPILib.Methods {
    public class MethodsSectionBase {
        internal VKAPI API;
        internal string Section;
        public MethodsSectionBase(VKAPI api, string section) {
            API = api;
            Section = section;
        }
    }
}
