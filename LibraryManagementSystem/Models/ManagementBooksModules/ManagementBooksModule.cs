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

        public enum ContinuedState
        {
            Continued,
            Discontinued
        }

        public enum FineType
        {
            LostBook,
            OverdueBook
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

        /// <summary>
        /// 图书检索功能
        /// </summary>
        /// <param name="bookInfoKind">检索属性</param>
        /// <param name="keyword">关键词</param>
        /// <returns></returns>
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
        /// <param name="bookId">图书编号</param>
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
            DataRowCollection rows = Sql.Read(
                $"SELECT BOOK_ID, READER_ID " +
                $"FROM BOOK_RESERVE " +
                $"WHERE (BOOK_OVERDUE_TIME < TO_DATE('{DateTime.Now.ToString("yyyy/MM/dd")}','yyyy/MM/dd'))");

            foreach (DataRow row in rows)
            {
                Sql.Execute(
                    $"DELETE FROM BOOK_RESERVE " +
                    $"WHERE(BOOK_ID = '{Convert.ToString(row["BOOK_ID"])}')");
            }
        }

        /// <summary>
        /// 借书超期处理功能
        /// </summary>
        public void ProcessBorrowingOverdue()
        {
            DataRowCollection rows = Sql.Read(
                $"SELECT * " +
                $"FROM BORROW " +
                $"WHERE(LATEST_RETURN_DATE < TO_DATE('{DateTime.Now.ToString("yyyy/MM/dd")}','yyyy/MM/dd'))");

            foreach (DataRow row in rows)
            {
                string overdueId = $"od{Sql.Read("SELECT * FROM OVERDUE").Count + 1}";
                Sql.Execute(
                    $"INSERT INTO OVERDUE " +
                    $"(READER_ID, BORROW_ID, OVERDUE_BORROW_DATE, OVERDUE_ID, OVERDUE_LATEST_DATE) " +
                    $"VALUES('{Convert.ToString(row["READER_ID"])}'," +
                    $"'{Convert.ToString(row["BORROW_ID"])}'," +
                    $"TO_DATE('{DateTime.Parse(Convert.ToString(row["BORROW_DATE"])).ToString("yyyy/MM/dd")}','yyyy/MM/dd'), " +
                    $"'{overdueId}'," +
                    $"TO_DATE('{DateTime.Parse(Convert.ToString(row["LATEST_RETURN_DATE"])).ToString("yyyy/MM/dd")}','yyyy/MM/dd'))");
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

        /// <summary>
        /// 超期受理功能
        /// </summary>
        /// <param name="readerId">读者帐号</param>
        /// <param name="bookId">图书编号</param>
        public void OverdueAccep(string readerId, string bookId)
        {
            string borrowId = string.Empty;
            string overdueId = string.Empty;

            DataRowCollection rows = Sql.Read(
               $"SELECT BORROW_ID " +
               $"FROM OVERDUE " +
               $"WHERE (READER_ID = '{readerId}')");

            if (rows.Count == 0)
            {
                throw new Exception();
            }

            foreach (DataRow row in rows)
            {
                DataRowCollection bookIdRows = Sql.Read(
                    $"SELECT BORROW_ID,BOOK_ID " +
                    $"FROM BORROW " +
                    $"WHERE BORROW_ID = '{row["BORROW_ID"]}'");

                if (bookIdRows.Count == 0)
                {
                    throw new Exception();
                }

                foreach (DataRow bookIdRow in bookIdRows)
                {
                    if (bookIdRow["BOOK_ID"] == bookId)
                    {
                        borrowId = Convert.ToString(bookIdRow["BORROW_ID"]);
                        overdueId = Convert.ToString(row["BORROW_ID"]);
                        break;
                    }
                }
            }

            DataRowCollection datas = Sql.Read($"SELECT PRICE FROM BOOK WHERE BOOK_ID = '{bookId}'");
            DataRow data = datas[0];
            int price = Convert.ToInt32(data["PRICE"]);
            string fineId = Convert.ToString(Convert.ToInt32(Sql.Read("SELECT * FROM FINE").Count) + 1);

            Sql.Execute($"INSERT INTO FINE " +
                $"(READER_ID, BOOK_ID, FINE_PRICE, FINE_ID, FINE_TYPE, PAYMENT_TIME) " +
                $"VALUES('{readerId}'," +
                $"'{bookId}'," +
                $"{price}," +
                $"'{fineId}'," +
                $"{Convert.ToInt32(FineType.OverdueBook)}," +
                $"'{DateTime.Now.ToString("yyyy/MM/dd")}')");

            Sql.Execute($"DELETE FROM OVERDUE WHERE OVERDUE_ID = '{overdueId}'");
            Sql.Execute($"DELETE FROM BORROW WHERE BORROW_ID = '{borrowId}'");
        }

        /// <summary>
        /// 挂失受理功能
        /// </summary>
        ///
        public void LossAccep(string bookId, string readerId)
        {
            DataRowCollection rows = Sql.Read(
                $"SELECT BOOK_ID" +
                $"FROM BOOK " +
                $"WHERE(BOOK_ID = '{bookId}')");

            string lossId = Convert.ToString(Convert.ToInt32(Sql.Read("SELECT * FROM LOSS").Count) + 1);

            Sql.Execute(
                $"INSERT INTO LOSS " +
                $"(	LOSS_ID, READER_ID,  BOOK_ID, LOSS_TIME) " +
                $"VALUES('{lossId}'," +
                $"'{readerId}'," +
                $"'{bookId}'," +
                $"'{DateTime.Now.ToString("yyyy/MM/dd")}'");

            string cancelId = Convert.ToString(Convert.ToInt32(Sql.Read("SELECT * FROM BOOKCANCEL").Count) + 1);
            string isbn = Convert.ToString(Convert.ToInt32(Sql.Read("SELECT ISBN FROM BOOK").Count));

            Sql.Execute(
                   $"INSERT INTO BOOKCANCEL " +
                   $"(BOOK_ID, CANCEL_ID, CANCEL_DATE, ISBN) " +
                   $"VALUES('{bookId}'," +
                   $"'{cancelId}'," +
                   $"'{DateTime.Now.ToString("yyyy/MM/dd")}'," +
                   $"'{isbn}'");

            Sql.Execute($"DELETE FROM BOOK WHERE BOOK_ID = '{bookId}'");

            string fineId = Convert.ToString(Convert.ToInt32(Sql.Read("SELECT * FROM FINE").Count) + 1);
            //TODO

            Sql.Execute(
                $"INSERT INTO FINE " +
                $"(	READER_ID, BOOK_ID, OVERDUE_TIME, FINE_ID, FINE_TYPE) " +
                $"VALUES('{readerId}'," +
                $"'{bookId}'," +
                $"'{"OVERDUE_TIME"}'," +
                $"'{fineId}'," +
                $"'{Convert.ToString(FineType.LostBook)}')");
        }

        /// <summary>
        /// 还书受理功能
        /// </summary>
        ///
        public void ReturnAccep(string readerId, string bookId)
        {
            DataRowCollection rows = Sql.Read(
                $"SELECT BOOK_ID" +
                $"FROM BORROW " +
                $"WHERE(BOOK_ID = '{bookId}')");

            string returnId = Convert.ToString(Convert.ToInt32(Sql.Read("SELECT * FROM RETURN").Count) + 1);
            string startDate = Convert.ToString(Convert.ToInt32(Sql.Read("SELECT BORROW_DATE FROM BORROW").Count));

            Sql.Execute(
                $"INSERT INTO RETURN " +
                $"(READER_ID, BOOK_ID, START_TIME, END_TIME, RETURN_ID) " +
                $"VALUES('{readerId}'," +
                $"'{bookId}'," +
                $"'{startDate}'," +
                $"'{DateTime.Now.ToString("yyyy/MM/dd")}'," +
                $"'{returnId}'");

            Sql.Execute($"DELETE FROM BORROW WHERE BOOK_ID = '{bookId}'");
        }

        /// <summary>
        /// 借书受理功能
        /// </summary>
        ///
        public void BorrowAccep(string readerId, string bookId)
        {
            DataRowCollection rows = Sql.Read(

                $"SELECT BOOK_ID , READER_ID" +
                $"FROM BOOK_REVERSE " +
                $"WHERE(BOOK_ID = '{bookId}')");

            if (rows.Count == 1)
            {
                if (rows[0]["READER_ID"] != readerId)
                {
                    throw new Exception("不一致");
                }

                Sql.Execute($"DELETE FROM BOOK_REVERSE WHERE BOOK_ID = '{bookId}'");
            }

            if (rows.Count > 2)
            {
                throw new Exception("找到信息2个或以上的，逻辑不对");
            }

            DataRowCollection rows_over = Sql.Read(
                 $"SELECT READER_ID" +
                 $"FROM OVERDUE " +
                 $"WHERE(READER_ID = '{readerId}')");

            if (rows_over.Count >= 1)
            {
                throw new Exception("先处理超期受理");
            }

            string borrowId = Convert.ToString(Convert.ToInt32(Sql.Read("SELECT * FROM BORROW").Count) + 1);

            Sql.Execute(
               $"INSERT INTO BORROW " +
               $"(READER_ID, BOOK_ID, BORROW_DATE, LATEST_RETURN_DATE , BORROW_ID , CONTINUED_STATE) " +
               $"VALUES('{readerId}'," +
               $"'{bookId}'," +
               $"'{DateTime.Now.ToString("yyyy/MM/dd")}'," +
               $"'{DateTime.Now.AddDays(14).ToString("yyyy/MM/dd")}'," +
               $"'{borrowId}'," +
               $"'{ Convert.ToString(ContinuedState.Continued)}'");
        }
    }
}
