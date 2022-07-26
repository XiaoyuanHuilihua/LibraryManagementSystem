﻿using LibraryManagementSystem.Models.UserManagementModules;
using LibraryManagementSystem.Models.ValueObject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LibraryManagementSystem.Tests
{
    /// <summary>
    /// 测试AdministratorModule类操作(函数)的测试类
    /// </summary>
    [TestClass]
    public class AdministratorModuleTest
    {
        private readonly AdministratorModule _administratorModule = new AdministratorModule();

        /// <summary>
        /// 以选择的信息搜索读者信息的测试
        /// </summary>
        [TestMethod]
        public void 以选择的信息搜索读者信息的测试()
        {
            var actuals = _administratorModule.SearchReaderInfo(AdministratorModule.ReaderInfoKind.ReaderId, "10");
            if (actuals.Count < 0)
            {
                throw new Exception();
            }
            foreach (var actual in actuals)
            {
                Assert.AreEqual("10", actual.ReaderId);
                Assert.AreEqual("Jackey Chen", actual.ReaderName);
                Assert.AreEqual("c123456", actual.Password);
                Assert.AreEqual(13750991571, actual.PhoneNumber);
                Assert.AreEqual("710100198712134422", actual.ReaderIdCard);
            }
            actuals.Clear();

            actuals = _administratorModule.SearchReaderInfo(AdministratorModule.ReaderInfoKind.ReaderName, "Jackey Chen");
            if (actuals.Count < 0)
            {
                throw new Exception();
            }
            foreach (var actual in actuals)
            {
                Assert.AreEqual("10", actual.ReaderId);
                Assert.AreEqual("Jackey Chen", actual.ReaderName);
                Assert.AreEqual("c123456", actual.Password);
                Assert.AreEqual(13750991571, actual.PhoneNumber);
                Assert.AreEqual("710100198712134422", actual.ReaderIdCard);
            }
            actuals.Clear();

            actuals = _administratorModule.SearchReaderInfo(AdministratorModule.ReaderInfoKind.PhoneNumber, "13750991571");
            if (actuals.Count < 0)
            {
                throw new Exception();
            }
            foreach (var actual in actuals)
            {
                Assert.AreEqual("10", actual.ReaderId);
                Assert.AreEqual("Jackey Chen", actual.ReaderName);
                Assert.AreEqual("c123456", actual.Password);
                Assert.AreEqual(13750991571, actual.PhoneNumber);
                Assert.AreEqual("710100198712134422", actual.ReaderIdCard);
            }
            actuals.Clear();

            actuals = _administratorModule.SearchReaderInfo(AdministratorModule.ReaderInfoKind.ReaderIdCard, "710100198712134422");
            if (actuals.Count < 0)
            {
                throw new Exception();
            }
            foreach (var actual in actuals)
            {
                Assert.AreEqual("10", actual.ReaderId);
                Assert.AreEqual("Jackey Chen", actual.ReaderName);
                Assert.AreEqual("c123456", actual.Password);
                Assert.AreEqual(13750991571, actual.PhoneNumber);
                Assert.AreEqual("710100198712134422", actual.ReaderIdCard);
            }
        }

        /// <summary>
        /// 是否正确修改读者信息的测试
        /// </summary>
        [TestMethod]
        public void 是否正确修改读者信息的测试()
        {
            //修改读者帐号
            var readerValueObject1 = new Reader("10", "Jackey Chen", 13750991571, "710100198712134422");
            _administratorModule.UpdateReaderInfo("11", "Jackey Chen", 13750991571, "710100198712134422");
            Assert.AreEqual("11", readerValueObject1.ReaderId);
            Assert.AreEqual("Jackey Chen", readerValueObject1.ReaderName);
            Assert.AreEqual(13750991571, readerValueObject1.PhoneNumber);
            Assert.AreEqual("710100198712134422", readerValueObject1.ReaderIdCard);

            //修改读者姓名
            var readerValueObject2 = new Reader("10", "Jackey Chen", 13750991571, "710100198712134422");
            _administratorModule.UpdateReaderInfo("10", "Jackey", 13750991571, "71010");
            Assert.AreEqual("10", readerValueObject2.ReaderId);
            Assert.AreEqual("Jackey", readerValueObject2.ReaderName);
            Assert.AreEqual(13750991571, readerValueObject2.PhoneNumber);
            Assert.AreEqual("710100198712134422", readerValueObject2.ReaderIdCard);

            //修改读者电话号码
            var readerValueObject4 = new Reader("10", "Jackey Chen", 13750991571, "710100198712134422");
            _administratorModule.UpdateReaderInfo("10", "Jackey Chen", 13750000000, "710100198712134422");
            Assert.AreEqual("10", readerValueObject4.ReaderId);
            Assert.AreEqual("Jackey Chen", readerValueObject4.ReaderName);
            Assert.AreEqual(13750000000, readerValueObject4.PhoneNumber);
            Assert.AreEqual("710100198712134422", readerValueObject4.ReaderIdCard);

            //修改读者身份证号
            var readerValueObject5 = new Reader("10", "Jackey Chen", 13750991571, "710100198712134422");
            _administratorModule.UpdateReaderInfo("10", "Jackey Chen", 13750991571, "710100198712130000");
            Assert.AreEqual("10", readerValueObject5.ReaderId);
            Assert.AreEqual("Jackey Chen", readerValueObject5.ReaderName);
            Assert.AreEqual(13750991571, readerValueObject5.PhoneNumber);
            Assert.AreEqual("710100198712130000", readerValueObject5.ReaderIdCard);
        }
    }
}
