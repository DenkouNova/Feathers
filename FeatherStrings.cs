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
    }
}
