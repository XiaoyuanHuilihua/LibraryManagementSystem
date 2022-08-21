using LibraryManagementSystem.Controllers;
using LibraryManagementSystem.Models.ManagementBooksModules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace LibraryManagementSystem.Tests
{
    /// <summary>
    /// 测试ManagementBooksModuleTest类操作(函数)的测试类
    /// </summary>
    [TestClass]
    public class ManagementBooksModuleTest
    {
        private readonly ManagementBooksModule _managementBooksModule = new ManagementBooksModule();

        [TestMethod]
        public void 图书录入功能()
        {
            _managementBooksModule.ResisterBook("TestBookBookBook", "TestISBN1234567890", "TestAuthor", "TestPublisher", DateTime.Parse("2022/01/01"), "TestBookDetail", 100);

            DataRowCollection dataCollection = Sql.Read("SELECT BOOK_ID FROM BOOK WHERE(ISBN = 'TestISBN1234567890')");
            string bookId = Convert.ToString(dataCollection[0]["BOOK_ID"]);

            DataRowCollection testDataRowCollection = Sql.Read("SELECT BOOK_ID, ISBN, BOOK_NAME, AUTHOR, PUBLISHER, PUBLISH_DATE, BOOK_DETAIL, PRICE FROM BOOK WHERE(ISBN = 'TestISBN1234567890')");

            if (testDataRowCollection.Count > 0)
            {
                string actialBookId = Convert.ToString(testDataRowCollection[0]["BOOK_ID"]);
                string actialBookName = Convert.ToString(testDataRowCollection[0]["BOOK_NAME"]);
                string actialISBN = Convert.ToString(testDataRowCollection[0]["ISBN"]);
                string actialAuthor = Convert.ToString(testDataRowCollection[0]["AUTHOR"]);
                string actialPublisher = Convert.ToString(testDataRowCollection[0]["PUBLISHER"]);
                string actialPublishDate = Convert.ToDateTime(testDataRowCollection[0]["PUBLISH_DATE"]).ToString("yyyy/MM/dd");
                string actialBookDetail = Convert.ToString(testDataRowCollection[0]["BOOK_DETAIL"]);
                int actialPrice = Convert.ToInt32(testDataRowCollection[0]["PRICE"]);

                Assert.AreEqual(bookId, actialBookId);
                Assert.AreEqual("TestBookBookBook", actialBookName);
                Assert.AreEqual("TestISBN1234567890", actialISBN);
                Assert.AreEqual("TestAuthor", actialAuthor);
                Assert.AreEqual("TestPublisher", actialPublisher);
                Assert.AreEqual("2022/01/01", actialPublishDate);
                Assert.AreEqual("TestBookDetail", actialBookDetail);
                Assert.AreEqual(100, actialPrice);
            }
            else
            {
                throw new Exception();
            }

            Sql.Execute($"DELETE FROM BOOK WHERE BOOK_ID = {bookId}");
        }
    }
}
