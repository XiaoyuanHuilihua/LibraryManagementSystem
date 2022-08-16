using LibraryManagementSystem.Models.UserManagementModules;
using LibraryManagementSystem.Models.ValueObject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibraryManagementSystem.Tests
{
    /// <summary>
    /// 测试Administrator类操作(函数)的测试类
    /// </summary>
    [TestClass]
    public class AdministratorTest
    {
        private readonly AdministratorModule _administratorModule = new AdministratorModule();

        /// <summary>
        /// 以读者Id搜索读者信息的测试
        /// </summary>
        [TestMethod]
        public void 以读者Id搜索读者信息的测试()
        {
            //テスト用
            var readerValueObject = new Reader("10", "Jackey Chen", "c123456", 13750991571, "710100198712134422");
            Assert.AreEqual("10", readerValueObject.ReaderId);
            Assert.AreEqual("Jackey Chen", readerValueObject.ReaderName);
            Assert.AreEqual("c123456", readerValueObject.Password);
            Assert.AreEqual(13750991571, readerValueObject.PhoneNumber);
            Assert.AreEqual("710100198712134422", readerValueObject.ReaderIdCard);
        }

        /// <summary>
        /// 是否正确修改读者信息的测试
        /// </summary>
        [TestMethod]
        public void 是否正确修改读者信息的测试()
        {
            //修改读者帐号
            var readerValueObject1 = new Reader("10", "Jackey Chen", "c123456", 13750991571, "710100198712134422");
            _administratorModule.UpdateReaderInfo(readerValueObject1, AdministratorModule.ReaderInfoKind.ReaderId, "11");
            Assert.AreEqual("11", readerValueObject1.ReaderId);
            Assert.AreEqual("Jackey Chen", readerValueObject1.ReaderName);
            Assert.AreEqual("c123456", readerValueObject1.Password);
            Assert.AreEqual(13750991571, readerValueObject1.PhoneNumber);
            Assert.AreEqual("710100198712134422", readerValueObject1.ReaderIdCard);

            //修改读者姓名
            var readerValueObject2 = new Reader("10", "Jackey Chen", "c123456", 13750991571, "710100198712134422");
            _administratorModule.UpdateReaderInfo(readerValueObject2, AdministratorModule.ReaderInfoKind.ReaderName, "Jackey");
            Assert.AreEqual("10", readerValueObject2.ReaderId);
            Assert.AreEqual("Jackey", readerValueObject2.ReaderName);
            Assert.AreEqual("c123456", readerValueObject2.Password);
            Assert.AreEqual(13750991571, readerValueObject2.PhoneNumber);
            Assert.AreEqual("710100198712134422", readerValueObject2.ReaderIdCard);

            //修改密码
            var readerValueObject3 = new Reader("10", "Jackey Chen", "c123456", 13750991571, "710100198712134422");
            _administratorModule.UpdateReaderInfo(readerValueObject3, AdministratorModule.ReaderInfoKind.Password, "654321c");
            Assert.AreEqual("10", readerValueObject3.ReaderId);
            Assert.AreEqual("Jackey Chen", readerValueObject3.ReaderName);
            Assert.AreEqual("654321c", readerValueObject3.Password);
            Assert.AreEqual(13750991571, readerValueObject3.PhoneNumber);
            Assert.AreEqual("710100198712134422", readerValueObject3.ReaderIdCard);

            //修改读者电话号码
            var readerValueObject4 = new Reader("10", "Jackey Chen", "c123456", 13750991571, "710100198712134422");
            _administratorModule.UpdateReaderInfo(readerValueObject4, AdministratorModule.ReaderInfoKind.PhoneNumber, 13750000000);
            Assert.AreEqual("10", readerValueObject4.ReaderId);
            Assert.AreEqual("Jackey Chen", readerValueObject4.ReaderName);
            Assert.AreEqual("c123456", readerValueObject4.Password);
            Assert.AreEqual(13750000000, readerValueObject4.PhoneNumber);
            Assert.AreEqual("710100198712134422", readerValueObject4.ReaderIdCard);

            //修改读者身份证号
            var readerValueObject5 = new Reader("10", "Jackey Chen", "c123456", 13750991571, "710100198712134422");
            _administratorModule.UpdateReaderInfo(readerValueObject5, AdministratorModule.ReaderInfoKind.ReaderIdCard, "710100198712130000");
            Assert.AreEqual("10", readerValueObject5.ReaderId);
            Assert.AreEqual("Jackey Chen", readerValueObject5.ReaderName);
            Assert.AreEqual("c123456", readerValueObject5.Password);
            Assert.AreEqual(13750991571, readerValueObject5.PhoneNumber);
            Assert.AreEqual("710100198712130000", readerValueObject5.ReaderIdCard);
        }

        /// <summary>
        /// 是否正确修改读者信息的测试
        /// </summary>
        [TestMethod]
        [Ignore]
        public void 查看读者借阅信息()
        {
            //TODO:现在还不会写这测试，实现“借书受理功能”之后才能设计
        }
    }
}
