using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Feathers;

namespace Feathers_Tests
{
    /// <summary>
    /// Summary description for FeatherLogger_UnitTests
    /// </summary>
    [TestClass]
    public class FeatherLogger_UnitTests
    {
        static string TEST_PATH = @"D:\Logger\Test_FeatherLogger";

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            if (Directory.Exists(TEST_PATH)) Directory.Delete(TEST_PATH, true);
        }

        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        [TestMethod]
        public void Test_FeatherLoggerCtorWithoutTimestamp()
        {
            string testName = MethodBase.GetCurrentMethod().Name;

            FeatherLogger fl = new FeatherLogger(
                FeatherLogger.TRACE_LEVEL_INFO,
                TEST_PATH + @"\" + testName,
                testName,
                false,
                "xml");

            Assert.IsTrue(Directory.Exists(TEST_PATH + @"\" + testName));
            Assert.IsFalse(!File.Exists(TEST_PATH + @"\" + testName + @"\" + testName + ".xml"));
        }

        [TestMethod]
        public void Test_FeatherLoggerCtorWithTimestamp()
        {
            string testName = MethodBase.GetCurrentMethod().Name;

            FeatherLogger fl = new FeatherLogger(
                FeatherLogger.TRACE_LEVEL_INFO,
                TEST_PATH + @"\" + testName,
                testName,
                true,
                "xml");

            int numberOfFilesCreatedWithTimestamp =
                Directory.GetFiles(TEST_PATH + @"\" + testName, testName + "20*.xml*").Length;
            int numberOfFilesCreatedTotal = 
                Directory.GetFiles(TEST_PATH + @"\" + testName, testName + "*").Length;

            Assert.IsTrue(numberOfFilesCreatedWithTimestamp == 1);
            Assert.IsFalse(numberOfFilesCreatedTotal != numberOfFilesCreatedWithTimestamp);
        }


        ///<summary>
        ///Test section opening and closing with tabs
        ///</summary>
        [TestMethod]
        public void Test_FeatherLoggerOpenCloseSection()
        {
            string testName = MethodBase.GetCurrentMethod().Name;
            string sectionName = "section";
            string logString = "log";

            FeatherLogger fl = new FeatherLogger(FeatherLogger.TRACE_LEVEL_INFO, TEST_PATH + @"\" + testName, testName, false, "xml");

            fl.Error(logString);
            fl.OpenSection(sectionName);
            fl.Error(logString);
            fl.OpenSection(sectionName);
            fl.Error(logString);
            fl.OpenSection(sectionName);
            fl.Error(logString);
            fl.CloseSection(sectionName);
            fl.Error(logString);
            fl.CloseSection(sectionName);
            fl.Error(logString);
            fl.CloseSection(sectionName);
            fl.Error(logString);

            using (StreamReader sr = new StreamReader(TEST_PATH + @"\" + testName + @"\" + testName + ".xml"))
            {
                sr.ReadLine(); // ignore first line, it's just an intro
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length), "ERROR: " + logString);
                Assert.AreEqual(sr.ReadLine().Substring(0, sectionName.Length + 2),
                    "<" + sectionName + ">");
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length + FeatherLogger.TAB_STRING.Length),
                    FeatherLogger.TAB_STRING + "ERROR: " + logString);
                Assert.AreEqual(sr.ReadLine().Substring(0, FeatherLogger.TAB_STRING.Length + sectionName.Length + 2),
                    FeatherLogger.TAB_STRING + "<" + sectionName + ">");
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length + FeatherLogger.TAB_STRING.Length * 2),
                    FeatherLogger.TAB_STRING + FeatherLogger.TAB_STRING + "ERROR: " + logString);
                Assert.AreEqual(sr.ReadLine().Substring(0, FeatherLogger.TAB_STRING.Length * 2 + sectionName.Length + 2),
                    FeatherLogger.TAB_STRING + FeatherLogger.TAB_STRING + "<" + sectionName + ">");
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length + FeatherLogger.TAB_STRING.Length * 3),

                    FeatherLogger.TAB_STRING + FeatherLogger.TAB_STRING + FeatherLogger.TAB_STRING + "ERROR: " + logString);

                Assert.AreEqual(sr.ReadLine().Substring(0, FeatherLogger.TAB_STRING.Length * 2 + sectionName.Length + 2 + 1),
                    FeatherLogger.TAB_STRING + FeatherLogger.TAB_STRING + "</" + sectionName + ">");
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length + FeatherLogger.TAB_STRING.Length * 2),
                    FeatherLogger.TAB_STRING + FeatherLogger.TAB_STRING + "ERROR: " + logString);
                Assert.AreEqual(sr.ReadLine().Substring(0, FeatherLogger.TAB_STRING.Length + sectionName.Length + 2 + 1),
                    FeatherLogger.TAB_STRING + "</" + sectionName + ">");
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length + FeatherLogger.TAB_STRING.Length),
                    FeatherLogger.TAB_STRING + "ERROR: " + logString);
                Assert.AreEqual(sr.ReadLine().Substring(0, sectionName.Length + 2 + 1),
                    "</" + sectionName + ">");
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length), "ERROR: " + logString);
            }
        }



        ///<summary>
        ///Test that Nothing-level tracing traces nothing
        ///</summary>
        [TestMethod]
        public void Test_FeatherLoggerNothing()
        {
            string testName = MethodBase.GetCurrentMethod().Name;
            string logString = "log";

            FeatherLogger fl = new FeatherLogger(FeatherLogger.TRACE_LEVEL_NOTHING, TEST_PATH + @"\" + testName, testName, false, "xml");

            fl.Error(logString);
            fl.Warn(logString);
            fl.Sql(logString);
            fl.Info(logString);
            fl.Extreme(logString);


            using (StreamReader sr = new StreamReader(TEST_PATH + @"\" + testName + @"\" + testName + ".xml"))
            {
                sr.ReadLine();
                Assert.IsNull(sr.ReadLine());
            }
        }



        ///<summary>
        ///Test that error tracing only traces errors
        ///</summary>
        [TestMethod]
        public void Test_FeatherLoggerError()
        {
            string testName = MethodBase.GetCurrentMethod().Name;
            string logString = "log";

            FeatherLogger fl = new FeatherLogger(FeatherLogger.TRACE_LEVEL_ERROR, TEST_PATH + @"\" + testName, testName, false, "xml");

            fl.Error(logString);
            fl.Warn(logString);
            fl.Sql(logString);
            fl.Info(logString);
            fl.Extreme(logString);

            using (StreamReader sr = new StreamReader(TEST_PATH + @"\" + testName + @"\" + testName + ".xml"))
            {
                sr.ReadLine();
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length), "ERROR: " + logString);
                Assert.IsNull(sr.ReadLine());
            }
        }



        ///<summary>
        ///Test that warn tracing traces both errors and warnings
        ///</summary>
        [TestMethod]
        public void Test_FeatherLoggerWarn()
        {
            string testName = MethodBase.GetCurrentMethod().Name;
            string logString = "log";

            FeatherLogger fl = new FeatherLogger(FeatherLogger.TRACE_LEVEL_WARN, TEST_PATH + @"\" + testName, testName, false, "xml");

            fl.Error(logString);
            fl.Warn(logString);
            fl.Sql(logString);
            fl.Info(logString);
            fl.Extreme(logString);

            using (StreamReader sr = new StreamReader(TEST_PATH + @"\" + testName + @"\" + testName + ".xml"))
            {
                sr.ReadLine();
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length), "ERROR: " + logString);
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length), "WARN : " + logString);
                Assert.IsNull(sr.ReadLine());
            }
        }



        ///<summary>
        ///Test that SQL tracing traces errors, warnings and SQL
        ///</summary>
        [TestMethod]
        public void Test_FeatherLoggerSQL()
        {
            string testName = MethodBase.GetCurrentMethod().Name;
            string logString = "log";

            FeatherLogger fl = new FeatherLogger(FeatherLogger.TRACE_LEVEL_SQL, TEST_PATH + @"\" + testName, testName, false, "xml");

            fl.Error(logString);
            fl.Warn(logString);
            fl.Sql(logString);
            fl.Info(logString);
            fl.Extreme(logString);

            using (StreamReader sr = new StreamReader(TEST_PATH + @"\" + testName + @"\" + testName + ".xml"))
            {
                sr.ReadLine();
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length), "ERROR: " + logString);
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length), "WARN : " + logString);
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length), "S Q L: " + logString);
                Assert.IsNull(sr.ReadLine());
            }
        }



        ///<summary>
        ///Test that Info tracing traces errors, warnings, SQL and Info
        ///</summary>
        [TestMethod]
        public void Test_FeatherLoggerInfo()
        {
            string testName = MethodBase.GetCurrentMethod().Name;
            string logString = "log";

            FeatherLogger fl = new FeatherLogger(FeatherLogger.TRACE_LEVEL_INFO, TEST_PATH + @"\" + testName, testName, false, "xml");

            fl.Error(logString);
            fl.Warn(logString);
            fl.Sql(logString);
            fl.Info(logString);
            fl.Extreme(logString);

            using (StreamReader sr = new StreamReader(TEST_PATH + @"\" + testName + @"\" + testName + ".xml"))
            {
                sr.ReadLine();
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length), "ERROR: " + logString);
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length), "WARN : " + logString);
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length), "S Q L: " + logString);
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length), "INFO : " + logString);
                Assert.IsNull(sr.ReadLine());
            }


        }



        ///<summary>
        ///Test that Info tracing traces errors, warnings, SQL, Info and Extreme
        ///</summary>
        [TestMethod]
        public void Test_FeatherLoggerExtreme()
        {
            string testName = MethodBase.GetCurrentMethod().Name;
            string logString = "log";

            FeatherLogger fl = new FeatherLogger(FeatherLogger.TRACE_LEVEL_EXTREME, TEST_PATH + @"\" + testName, testName, false, "xml");

            fl.Error(logString);
            fl.Warn(logString);
            fl.Sql(logString);
            fl.Info(logString);
            fl.Extreme(logString);


            using (StreamReader sr = new StreamReader(TEST_PATH + @"\" + testName + @"\" + testName + ".xml"))
            {
                sr.ReadLine();

                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length), "ERROR: " + logString);
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length), "WARN : " + logString);
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length), "S Q L: " + logString);
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length), "INFO : " + logString);
                Assert.AreEqual(sr.ReadLine().Substring(0, 7 + logString.Length), "EXTRM: " + logString);
                Assert.IsNull(sr.ReadLine());
            }
        }
        

    }
}
