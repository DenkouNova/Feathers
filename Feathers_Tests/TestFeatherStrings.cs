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


        [TestMethod]
        public void Test_FeatherStringsWithInt()
        {
            int a = 10;
            Assert.AreEqual(FeatherStrings.TraceString(a), "10");
        }


        [TestMethod]
        public void Test_FeatherStringsWithNullableInt()
        {
            int? b = 20;
            Assert.AreEqual(FeatherStrings.TraceString(b), "20");
        }


        [TestMethod]
        public void Test_FeatherStringsWithNullInt()
        {
            int? nullValue = null;
            Assert.AreEqual(FeatherStrings.TraceString(nullValue), "(null)");
        }


        [TestMethod]
        public void Test_FeatherStringsWithFloat()
        {
            float c = 30;
            Assert.AreEqual(FeatherStrings.TraceString(c), "30");
        }


        [TestMethod]
        public void Test_FeatherStringsWithNullableFloat()
        {
            float? d = 40;
            Assert.AreEqual(FeatherStrings.TraceString(d), "40");
        }


        [TestMethod]
        public void Test_FeatherStringsWithNullFloat()
        {
            float? nullValue = null;
            Assert.AreEqual(FeatherStrings.TraceString(nullValue), "(null)");
        }


        [TestMethod]
        public void Test_FeatherStringsWithString()
        {
            string e = "50";
            Assert.AreEqual(FeatherStrings.TraceString(e), "'50'");
        }


        [TestMethod]
        public void Test_FeatherStringsWithNullString()
        {
            string nullValue = null;
            Assert.AreEqual(FeatherStrings.TraceString(nullValue), "(null)");
        }

    }
}
