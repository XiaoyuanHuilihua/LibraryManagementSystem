using LibraryManagementSystem.Models.UserManagementModules;
using LibraryManagementSystem.Models.ValueObject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibraryManagementSystem.Tests
{
    [TestClass]
    public class AdministratorTest
    {
        private readonly AdministratorModule _administratorModule = new AdministratorModule();

        [TestMethod]
        public void 読者IDを使用した読者情報の検索テスト()
        {
            //テスト用
            var readerValueObject = new Reader("10", "Jackey Chen", "c123456", 13750991571, "710100198712134422");
            Assert.AreEqual("10", readerValueObject.ReaderId);
            Assert.AreEqual("Jackey Chen", readerValueObject.ReaderName);
            Assert.AreEqual("c123456", readerValueObject.Password);
            Assert.AreEqual(13750991571, readerValueObject.PhoneNumber);
            Assert.AreEqual("710100198712134422", readerValueObject.ReaderIdCard);
        }
    }
}
