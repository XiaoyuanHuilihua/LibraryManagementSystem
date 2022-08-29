using LibraryManagementSystem.Controllers;
using LibraryManagementSystem.Models.ValueObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static LibraryManagementSystem.Models.ValueObject.Book;

namespace LibraryManagementSystem.Models.ManagementBooksModules
{
    public class ManagementBooksModule
    {
        public enum BookInfoKind
        {
            BookId,
            ISBN,
            BookName,
            Author,
            Publisher,
        }

        /// <summary>
        /// 图书录入功能
        /// </summary>
        public void ResisterBook(string bookId, string bookName, string isbn, string author, string publisher, DateTime publishDate, string bookDetail, int price, string pictureList = null)
        {
            var book = new Book(
                bookId,
                isbn,
                bookName,
                author,
                publisher,
                publishDate,
                bookDetail,
                price);

            Sql.Execute(
                $"INSERT INTO BOOK " +
                $"(BOOK_ID, ISBN, BOOK_NAME, AUTHOR, PUBLISHER, PUBLISH_DATE, BOOK_DETAIL, PRICE) " +
                $"VALUES('{book.BookId}'," +
                $"'{book.ISBN}'," +
                $"'{book.BookName}'," +
                $"'{book.Author}'," +
                $"'{book.Publisher}'," +
                $"'{book.PublishDate.ToString("yyyy/MM/dd")}'," +
                $"'{book.BookDetail}'," +
                $"{book.Price})");
        }

        public List<Book> SearchBookInfo(BookInfoKind bookInfoKind, dynamic keyword)
        {
            DataRowCollection rows;

            switch (bookInfoKind)
            {
                case (BookInfoKind.BookId):
                    rows = Sql.Read(
                        $"SELECT BOOK_ID, ISBN, BOOK_NAME, AUTHOR, PUBLISHER, PUBLISH_DATE, BOOK_DETAIL " +
                        $"FROM BOOK " +
                        $"WHERE(BOOK_ID = '{keyword}')");
                    break;

                case (BookInfoKind.ISBN):
                    rows = Sql.Read(
                        $"SELECT BOOK_ID, ISBN, BOOK_NAME, AUTHOR, PUBLISHER, PUBLISH_DATE, BOOK_DETAIL " +
                        $"FROM BOOK " +
                        $"WHERE(ISBN = '{keyword}')");

                    break;

                case (BookInfoKind.BookName):
                    rows = Sql.Read(
                        $"SELECT BOOK_ID, ISBN, BOOK_NAME, AUTHOR, PUBLISHER, PUBLISH_DATE, BOOK_DETAIL " +
                        $"FROM BOOK " +
                        $"WHERE(BOOK_NAME = '{keyword}')");

                    break;

                case (BookInfoKind.Author):
                    rows = Sql.Read(
                        $"SELECT BOOK_ID, ISBN, BOOK_NAME, AUTHOR, PUBLISHER, PUBLISH_DATE, BOOK_DETAIL " +
                        $"FROM BOOK " +
                        $"WHERE(AUTHOR = '{keyword}')");
                    break;

                case (BookInfoKind.Publisher):
                    rows = Sql.Read(
                        $"SELECT BOOK_ID, ISBN, BOOK_NAME, AUTHOR, PUBLISHER, PUBLISH_DATE, BOOK_DETAIL " +
                        $"FROM BOOK " +
                        $"WHERE(PUBLISHER = '{keyword}')");
                    break;

                default:
                    throw new Exception();
                    break;
            }

            if (rows.Count == 0)
            {
                throw new Exception("搜索结果为0。");
            }

            var lists = new List<Book>();

            for (int i = 0; i < rows.Count; i++)
            {
                //转换：字符串->日期类型
                string dateTimeString = Convert.ToString(rows[i]["PUBLISH_DATE"]);
                DateTime publishDate = DateTime.Parse(dateTimeString);

                lists.Add(new Book(
                    Convert.ToString(rows[i]["BOOK_ID"]),
                    Convert.ToString(rows[i]["ISBN"]),
                    Convert.ToString(rows[i]["BOOK_NAME"]),
                    Convert.ToString(rows[i]["AUTHOR"]),
                    Convert.ToString(rows[i]["PUBLISHER"]),
                    publishDate,
                    Convert.ToString(rows[i]["BOOK_DETAIL"])
                    ));
            }

            return lists;
        }

        /// <summary>
        /// 图书注销功能
        /// </summary>
        public void BookCancellation(dynamic bookId)
        {
            DataRowCollection rows = Sql.Read(
                $"SELECT BOOK_ID, ISBN, BOOK_NAME, AUTHOR, PUBLISHER, PUBLISH_DATE, LEND_RESERVE, BOOK_DETAIL, PICTURE_LIST, PRICE " +
                $"FROM BOOK " +
                $"WHERE(BOOK_ID = '{bookId}')");

            if (rows.Count == 0)
            {
                throw new Exception("无数据");
            }
            if (rows.Count > 1)
            {
                throw new Exception("逻辑不对");
            }

            Sql.Execute($"DELETE FROM BOOK WHERE BOOK_ID = '{bookId}'");

            string cancelId = Convert.ToString(Sql.Read($"SELECT * FROM BOOKCANCEL").Count + 1);
            var bookCancelObj = new Bookcancel(bookId, cancelId, DateTime.Now, Convert.ToString(rows[0]["ISBN"]));

            Sql.Execute(
                $"INSERT INTO BOOKCANCEL " +
                $"(BOOK_ID, CANCEL_ID, CANCEL_DATE, ISBN) " +
                $"VALUES('{bookCancelObj.BookId}'," +
                $"'{bookCancelObj.CancelId}'," +
                $"'{bookCancelObj.CancelDate.ToString("yyyy/MM/dd")}'," +
                $"'{bookCancelObj.ISBN}')");
        }

        /// <summary>
        /// 预约超期处理功能
        /// </summary>
        public void ProcessReserveOverdue()
        {
            //图书管理员可以定期检索超期预约。从图书预约表中删除相关信息，通知读者预约超期消息
            //所需数据：	图书预约到期时间，读者账号，图书编号

            //TODO 通知读者预约超期消息是通过邮箱地址通知，还是在网站内提示？
            DataRowCollection rows = Sql.Read(
                $"SELECT BOOK_ID, READER_ID " +
                $"FROM BOOL_RESERVE " +
                $"WHERE(BOOK_OVERDUE_TIME < {DateTime.Now.ToString("yyyy/MM/dd")})");

            foreach (DataRow row in rows)
            {
                Sql.Execute(
                    $"DELETE FROM BOOL_RESERVE " +
                    $"WHERE(BOOK_ID = '{Convert.ToString(row["BOOK_ID"])}')");
            }
        }

        /// <summary>
        /// 借书超期处理功能
        /// </summary>
        public void ProcessBorrowingOverdue()
        {
            //图书管理员可以定期检索超期借阅。从借阅表中取得相关信息并录入到超期表，通知读者借书超期消息
            //所需数据： 最晚还书日期，读者账号，图书编号
            DataRowCollection rows = Sql.Read(
                $"SELECT BOOK_ID, READER_ID " +
                $"FROM BORROW " +
                $"WHERE(LATEST_RETURN_DATE < {DateTime.Now.ToString("yyyy/MM/dd")})");

            foreach (DataRow row in rows)
            {
                Sql.Execute(
                    $"INSERT INTO OVERDUE " +
                    $"(READER_ID, BOOK_ID, OVERDUE_BORROW_DATE, OVERDUE_ID, OVERDUE_LATEST_DATE) " +
                    $"VALUES('{row["READER_ID"]}'," +
                    $"'{row["BOOK_ID"]}'," +
                    $"'{Convert.ToDateTime(row["OVERDUE_BORROW_DATE"]).ToString("yyyy/MM/dd")}'," +
                    $"'{row["OVERDUE_ID"]}'" +
                    $"'{row["OVERDUE_LATEST_DATE"]}')");
            }
        }

        /// <summary>
        /// 图书损坏处理功能
        /// </summary>
        public void DealWithDamagedBooks(string bookId, string cancelId, string reason = null)
        {
            //图书管理员对归还时损坏的图书进行罚款处理，从借书表中删除相关信息并录入到注销表，对图书信息进行修改
            //所需数据：	读者账号，图书编号，图书标价
            var bookInfo = SearchBookInfo(BookInfoKind.BookId, bookId);

            if (bookInfo.Count == 0)
            {
                throw new Exception("无数据");
            }
            if (bookInfo.Count > 1)
            {
                throw new Exception("逻辑不对");
            }

            Book cancelBook = new Book(
                bookInfo[0].BookId,
                bookInfo[0].ISBN,
                bookInfo[0].BookName,
                bookInfo[0].Author,
                bookInfo[0].Publisher,
                bookInfo[0].PublishDate,
                bookInfo[0].BookDetail);

            Sql.Execute(
                    $"INSERT INTO BOOKCANCEL " +
                    $"(BOOK_ID, CANCEL_ID, CANCEL_DATE, ISBN) " +
                    $"VALUES('{cancelBook.BookId}'," +
                    $"'{cancelId}'," +
                    $"'{Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"))}'," +
                    $"'{cancelBook.ISBN}'" +
                    $"'{reason}')");

            Sql.Execute(
                    $"DELETE FROM BOOK " +
                    $"WHERE(BOOK_ID = '{cancelBook.BookId}')");
        }
    }
}
