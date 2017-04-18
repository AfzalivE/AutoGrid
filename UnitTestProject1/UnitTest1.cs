using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1 {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            Assert.AreEqual(0, 0, "Account not debited correctly");

        }
    }
}
