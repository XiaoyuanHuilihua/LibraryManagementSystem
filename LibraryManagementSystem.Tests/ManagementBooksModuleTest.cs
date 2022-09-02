using LibraryManagementSystem.Controllers;
using LibraryManagementSystem.Models.ManagementBooksModules;
using LibraryManagementSystem.Models.ReaderModule;
using LibraryManagementSystem.Models.UserManagementModules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using static LibraryManagementSystem.Models.ManagementBooksModules.ManagementBooksModule;

namespace LibraryManagementSystem.Tests
{
    /// <summary>
    /// 测试ManagementBooksModuleTest类操作(函数)的测试类
    /// </summary>
    [TestClass]
    public class ManagementBooksModuleTest
    {
        /// <summary>
        /// 书籍管理模块类
        /// </summary>
        private readonly ManagementBooksModule _managementBooksModule =
            new ManagementBooksModule();

        /// <summary>
        /// 管理员模块类
        /// </summary>
        private readonly AdministratorModule _administratorModule =
            new AdministratorModule();

        /// <summary>
        /// 读者模块类
        /// </summary>
        private readonly ReaderModule _readerModule =
            new ReaderModule();

        /// <summary>
        /// 图书录入功能测试
        /// </summary>
        [TestMethod]
        public void 图书录入功能测试()
        {
            _managementBooksModule.ResisterBook(
                "TestBookId123",
                "TestBookBookBook",
                "TestISBN1234567890",
                "TestAuthor",
                "TestPublisher",
                DateTime.Parse("2022/01/01"),
                "TestBookDetail",
                100);

            DataRowCollection testDataRowCollection =
                Sql.Read(
                    "SELECT BOOK_ID, ISBN, BOOK_NAME, AUTHOR, PUBLISHER, PUBLISH_DATE, BOOK_DETAIL, PRICE " +
                    "FROM BOOK " +
                    "WHERE(ISBN = 'TestISBN1234567890')");

            if (testDataRowCollection.Count > 0)
            {
                string actualBookId = Convert.ToString(testDataRowCollection[0]["BOOK_ID"]);
                string actualBookName = Convert.ToString(testDataRowCollection[0]["BOOK_NAME"]);
                string actualISBN = Convert.ToString(testDataRowCollection[0]["ISBN"]);
                string actualAuthor = Convert.ToString(testDataRowCollection[0]["AUTHOR"]);
                string actualPublisher = Convert.ToString(testDataRowCollection[0]["PUBLISHER"]);
                string actualPublishDate = Convert.ToDateTime(testDataRowCollection[0]["PUBLISH_DATE"]).ToString("yyyy/MM/dd");
                string actualBookDetail = Convert.ToString(testDataRowCollection[0]["BOOK_DETAIL"]);
                int actualPrice = Convert.ToInt32(testDataRowCollection[0]["PRICE"]);

                Assert.AreEqual("TestBookId123", actualBookId);
                Assert.AreEqual("TestBookBookBook", actualBookName);
                Assert.AreEqual("TestISBN1234567890", actualISBN);
                Assert.AreEqual("TestAuthor", actualAuthor);
                Assert.AreEqual("TestPublisher", actualPublisher);
                Assert.AreEqual("2022/01/01", actualPublishDate);
                Assert.AreEqual("TestBookDetail", actualBookDetail);
                Assert.AreEqual(100, actualPrice);
            }
            else
            {
                throw new Exception();
            }

            Sql.Execute(
                $"DELETE FROM BOOK " +
                $"WHERE BOOK_ID = 'TestBookId123'");
        }

        /// <summary>
        ///图书检索功能测试
        /// </summary>
        [TestMethod]
        public void 图书检索功能测试()
        {
            var actuals = _managementBooksModule.SearchBookInfo(BookInfoKind.BookId, "3");
            if (actuals.Count < 0)
            {
                throw new Exception();
            }

            foreach (var actual in actuals)
            {
                Assert.AreEqual("3", actual.BookId);
                Assert.AreEqual("978-1540528247", actual.ISBN);
                Assert.AreEqual("Hard Times", actual.BookName);
                Assert.AreEqual("Charles Dickens", actual.Author);
                Assert.AreEqual("CreateSpace", actual.Publisher);
                Assert.AreEqual(DateTime.Parse("2016/11/20"), actual.PublishDate);
                Assert.AreEqual("Hard Times Charles Dickens Hard Times ？ For These Times (commonly known as Hard Times) is the tenth novel by Charles Dickens, first published in 1854. The book appraises English society and highlights the social and economic pressures of the times. The novel was published as a serial in Dickens's weekly publication, Household Words. Sales were highly responsive and encouraging for Dickens who remarked that he was \"Three parts mad, and the fourth delirious, with perpetual rushing at Hard Times\". The novel was serialised, in twenty weekly parts, between 1 April and 12 August 1854. It sold well, and a complete volume was published in August, totalling 110,000 words. Another related novel, North and South by Elizabeth Gaskell, was also published in this magazine.", actual.BookDetail);
            }
            actuals.Clear();

            actuals = _managementBooksModule.SearchBookInfo(BookInfoKind.ISBN, "978-1540528247");
            if (actuals.Count < 0)
            {
                throw new Exception();
            }

            foreach (var actual in actuals)
            {
                Assert.AreEqual("3", actual.BookId);
                Assert.AreEqual("978-1540528247", actual.ISBN);
                Assert.AreEqual("Hard Times", actual.BookName);
                Assert.AreEqual("Charles Dickens", actual.Author);
                Assert.AreEqual("CreateSpace", actual.Publisher);
                Assert.AreEqual(DateTime.Parse("2016/11/20"), actual.PublishDate);
                Assert.AreEqual("Hard Times Charles Dickens Hard Times ？ For These Times (commonly known as Hard Times) is the tenth novel by Charles Dickens, first published in 1854. The book appraises English society and highlights the social and economic pressures of the times. The novel was published as a serial in Dickens's weekly publication, Household Words. Sales were highly responsive and encouraging for Dickens who remarked that he was \"Three parts mad, and the fourth delirious, with perpetual rushing at Hard Times\". The novel was serialised, in twenty weekly parts, between 1 April and 12 August 1854. It sold well, and a complete volume was published in August, totalling 110,000 words. Another related novel, North and South by Elizabeth Gaskell, was also published in this magazine.", actual.BookDetail);
            }
            actuals.Clear();

            actuals = _managementBooksModule.SearchBookInfo(BookInfoKind.BookName, "Hard Times");
            if (actuals.Count < 0)
            {
                throw new Exception();
            }

            foreach (var actual in actuals)
            {
                Assert.AreEqual("3", actual.BookId);
                Assert.AreEqual("978-1540528247", actual.ISBN);
                Assert.AreEqual("Hard Times", actual.BookName);
                Assert.AreEqual("Charles Dickens", actual.Author);
                Assert.AreEqual("CreateSpace", actual.Publisher);
                Assert.AreEqual(DateTime.Parse("2016/11/20"), actual.PublishDate);
                Assert.AreEqual("Hard Times Charles Dickens Hard Times ？ For These Times (commonly known as Hard Times) is the tenth novel by Charles Dickens, first published in 1854. The book appraises English society and highlights the social and economic pressures of the times. The novel was published as a serial in Dickens's weekly publication, Household Words. Sales were highly responsive and encouraging for Dickens who remarked that he was \"Three parts mad, and the fourth delirious, with perpetual rushing at Hard Times\". The novel was serialised, in twenty weekly parts, between 1 April and 12 August 1854. It sold well, and a complete volume was published in August, totalling 110,000 words. Another related novel, North and South by Elizabeth Gaskell, was also published in this magazine.", actual.BookDetail);
            }
            actuals.Clear();

            actuals = _managementBooksModule.SearchBookInfo(BookInfoKind.Author, "Charles Dickens");
            if (actuals.Count < 0)
            {
                throw new Exception();
            }

            foreach (var actual in actuals)
            {
                Assert.AreEqual("3", actual.BookId);
                Assert.AreEqual("978-1540528247", actual.ISBN);
                Assert.AreEqual("Hard Times", actual.BookName);
                Assert.AreEqual("Charles Dickens", actual.Author);
                Assert.AreEqual("CreateSpace", actual.Publisher);
                Assert.AreEqual(DateTime.Parse("2016/11/20"), actual.PublishDate);
                Assert.AreEqual("Hard Times Charles Dickens Hard Times ？ For These Times (commonly known as Hard Times) is the tenth novel by Charles Dickens, first published in 1854. The book appraises English society and highlights the social and economic pressures of the times. The novel was published as a serial in Dickens's weekly publication, Household Words. Sales were highly responsive and encouraging for Dickens who remarked that he was \"Three parts mad, and the fourth delirious, with perpetual rushing at Hard Times\". The novel was serialised, in twenty weekly parts, between 1 April and 12 August 1854. It sold well, and a complete volume was published in August, totalling 110,000 words. Another related novel, North and South by Elizabeth Gaskell, was also published in this magazine.", actual.BookDetail);
            }
            actuals.Clear();

            actuals = _managementBooksModule.SearchBookInfo(BookInfoKind.Publisher, "CreateSpace");
            if (actuals.Count < 0)
            {
                throw new Exception();
            }

            foreach (var actual in actuals)
            {
                Assert.AreEqual("3", actual.BookId);
                Assert.AreEqual("978-1540528247", actual.ISBN);
                Assert.AreEqual("Hard Times", actual.BookName);
                Assert.AreEqual("Charles Dickens", actual.Author);
                Assert.AreEqual("CreateSpace", actual.Publisher);
                Assert.AreEqual(DateTime.Parse("2016/11/20"), actual.PublishDate);
                Assert.AreEqual("Hard Times Charles Dickens Hard Times ？ For These Times (commonly known as Hard Times) is the tenth novel by Charles Dickens, first published in 1854. The book appraises English society and highlights the social and economic pressures of the times. The novel was published as a serial in Dickens's weekly publication, Household Words. Sales were highly responsive and encouraging for Dickens who remarked that he was \"Three parts mad, and the fourth delirious, with perpetual rushing at Hard Times\". The novel was serialised, in twenty weekly parts, between 1 April and 12 August 1854. It sold well, and a complete volume was published in August, totalling 110,000 words. Another related novel, North and South by Elizabeth Gaskell, was also published in this magazine.", actual.BookDetail);
            }
            actuals.Clear();
        }

        /// <summary>
        /// 图书注销功能测试
        /// </summary>
        [TestMethod]
        public void 图书注销功能测试()
        {
            _managementBooksModule.ResisterBook(
                "987654321",
                "TestBookBookBook",
                "TestISBN1234567890",
                "TestAuthor",
                "TestPublisher",
                DateTime.Parse("2022/01/01"),
                "TestBookDetail",
                100);

            DataRowCollection testResister =
                Sql.Read(
                    "SELECT BOOK_ID, ISBN, BOOK_NAME, AUTHOR, PUBLISHER, PUBLISH_DATE, BOOK_DETAIL, PRICE " +
                    "FROM BOOK " +
                    "WHERE(ISBN = 'TestISBN1234567890')");

            if (testResister.Count > 0)
            {
                string actualBookId = Convert.ToString(testResister[0]["BOOK_ID"]);
                string actualBookName = Convert.ToString(testResister[0]["BOOK_NAME"]);
                string actualISBN = Convert.ToString(testResister[0]["ISBN"]);
                string actualAuthor = Convert.ToString(testResister[0]["AUTHOR"]);
                string actualPublisher = Convert.ToString(testResister[0]["PUBLISHER"]);
                string actualPublishDate = Convert.ToDateTime(testResister[0]["PUBLISH_DATE"]).ToString("yyyy/MM/dd");
                string actualBookDetail = Convert.ToString(testResister[0]["BOOK_DETAIL"]);
                int actualPrice = Convert.ToInt32(testResister[0]["PRICE"]);

                Assert.AreEqual("987654321", actualBookId);
                Assert.AreEqual("TestBookBookBook", actualBookName);
                Assert.AreEqual("TestISBN1234567890", actualISBN);
                Assert.AreEqual("TestAuthor", actualAuthor);
                Assert.AreEqual("TestPublisher", actualPublisher);
                Assert.AreEqual("2022/01/01", actualPublishDate);
                Assert.AreEqual("TestBookDetail", actualBookDetail);
                Assert.AreEqual(100, actualPrice);
            }
            else
            {
                throw new Exception();
            }

            _managementBooksModule.BookCancellation("987654321");

            DataRowCollection testDataRowCollection =
               Sql.Read(
                   $"SELECT BOOK_ID, ISBN, CANCEL_ID, CANCEL_DATE " +
                   $"FROM BOOKCANCEL " +
                   $"WHERE(BOOK_ID = '987654321')");

            if (testDataRowCollection.Count > 0)
            {
                string actualBookId = Convert.ToString(testDataRowCollection[0]["BOOK_ID"]);
                string actualCancelId = Convert.ToString(testDataRowCollection[0]["CANCEL_ID"]);
                string actualISBN = Convert.ToString(testDataRowCollection[0]["ISBN"]);
                string actualCancelDate = Convert.ToDateTime(testDataRowCollection[0]["CANCEL_DATE"]).ToString("yyyy/MM/dd");

                Assert.AreEqual("987654321", actualBookId);
                Assert.AreEqual("1", actualCancelId);
                Assert.AreEqual("TestISBN1234567890", actualISBN);
                Assert.AreEqual(DateTime.Now.ToString("yyyy/MM/dd"), actualCancelDate);
            }
            else
            {
                throw new Exception();
            }

            Sql.Execute(
                $"DELETE FROM BOOKCANCEL " +
                $"WHERE BOOK_ID = '987654321'");
        }

        /// <summary>
        /// 预约超期处理功能测试
        /// </summary>
        [TestMethod]
        public void 预约超期处理功能测试()
        {
            _readerModule.ReserveBook("12", "9");
            Sql.Execute($"UPDATE BOOK_RESERVE SET BOOK_OVERDUE_TIME = '2022/01/01' WHERE (BOOK_ID = '9')");
            _managementBooksModule.ProcessReserveOverdue();
            string actual = Convert.ToString(Sql.Read("SELECT * FROM BOOK_RESERVE WHERE(BOOK_ID = '9')").Count);
            Assert.AreEqual("0", actual);
        }

        /// <summary>
        /// 借书超期处理功能测试
        /// </summary>
        [TestMethod]
        public void 借书超期处理功能测试()
        {
            _managementBooksModule.ProcessBorrowingOverdue();

            DataRowCollection actual = Sql.Read("SELECT * FROM OVERDUE WHERE(BORROW_ID = 'br100')");
            Assert.IsTrue(actual.Count != 0);
        }

        /// <summary>
        /// 超期受理功能测试
        /// </summary>
        [TestMethod]
        public void 超期受理功能测试()
        {
            _managementBooksModule.OverdueAccep("14", "2");
            var rowsFINE = Sql.Read($"SELECT * FROM FINE WHERE (READER_ID = '14' AND BOOK_ID = '2')");
            Assert.AreEqual(1, rowsFINE.Count);
        }
    }
}
