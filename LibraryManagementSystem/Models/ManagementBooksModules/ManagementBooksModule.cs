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
        public void ResisterBook(string bookName, string isbn, string author, string publisher, DateTime publishDate, string bookDetail, int price, string pictureList = null)
        {
            //生成图书编号
            int bookId = Sql.Read("SELECT * FROM BOOK").Count + 1;

            //TODO:要删除quantity属性
            var book = new Book(
                bookId.ToString(),
                isbn,
                bookName,
                author,
                publisher,
                publishDate,
                bookDetail,
                pictureList,
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
    }
}
