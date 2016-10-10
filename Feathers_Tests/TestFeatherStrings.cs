using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Feathers;

namespace Feathers_Tests
{
    /// <summary>
    /// Summary description for UnitTests_FeatherStrings
    /// </summary>
    [TestClass]
    public class TestFeatherStrings
    {
        [TestMethod]
        public void Test_FeatherStringsReplaceWhitespaceSpecialCharacters()
        {
            string s = "jiroagpjioyadguilfagu4390qpr09eaut90234-j7389godjagfoigAEGERAGJIEA%ILJ\"/%J/%'''";
            Assert.AreEqual(s, FeatherStrings.ReplaceWhitespaceSpecialCharacters(s));

            s = "\r\n\t";
            Assert.AreEqual(@"{\r}{\n}{\t}", FeatherStrings.ReplaceWhitespaceSpecialCharacters(s));

            s = @"\r\n\t";
            Assert.AreEqual(s, FeatherStrings.ReplaceWhitespaceSpecialCharacters(s));
        }
    }
}
