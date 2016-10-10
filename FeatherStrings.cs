using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Feathers
{
    public class FeatherStrings
    {
        public static string ReplaceWhitespaceSpecialCharacters(string s)
        {
            return s
                .Replace("\r", @"{\r}")
                .Replace("\n", @"{\n}")
                .Replace("\t", @"{\t}");
        }

        public static string TraceString(int i)
        {
            return i.ToString();
        }

        public static string TraceString(int? i)
        {
            return (i == null ? "(null)" : i.ToString());
        }

        public static string TraceString(float f)
        {
            return f.ToString();
        }

        public static string TraceString(float? f)
        {
            return (f == null ? "(null)" : f.ToString());
        }

        public static string TraceString(string s)
        {
            return (s == null ? "(null)" : "'" + s + "'");
        }
    }
}
