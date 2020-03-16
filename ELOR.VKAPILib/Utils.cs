using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ELOR.VKAPILib {
    internal static class Utils {
        public static string ToEnumMemberAttribute(this Enum @enum) {
            var attr = @enum.GetType().GetTypeInfo().DeclaredMembers.FirstOrDefault()?.GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();
            if (attr == null) return @enum.ToString();
            return attr.Value;
        }

        internal static bool IsNullOrEmpty(this List<int> list) {
            return list == null || list.Count == 0;
        }

        internal static bool IsNullOrEmpty(this List<string> list) {
            return list == null || list.Count == 0;
        }

        internal static string Combine(this List<int> items, char sym = ',') {
            string s = sym.ToString();

            foreach(int i in items) {
                s += sym + i.ToString();
            }

            return s.Substring(1);
        }

        internal static string Combine(this List<string> items, char sym = ',') {
            string s = sym.ToString();

            foreach (string str in items) {
                s += sym + str;
            }

            return s.Substring(1);
        }
    }
}
